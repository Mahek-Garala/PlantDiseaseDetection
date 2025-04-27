// export default Feed;
import React, { useEffect, useState } from "react";
import axios from "axios";
import {
  FaThumbsUp,
  FaCommentAlt,
  FaWhatsapp,
  FaCamera,
  FaPaperPlane,
  FaLeaf,
} from "react-icons/fa";
import Navbar from "./Navbar";

const Feed = () => {
  const [posts, setPosts] = useState([]);
  const [replyInputs, setReplyInputs] = useState({});
  const [newPost, setNewPost] = useState({
    plantName: "",
    description: "",
    imageFile: null,
  });
  const [previewImage, setPreviewImage] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [showReplies, setShowReplies] = useState({});

  const getUser = () => {
    const userData = localStorage.getItem("user");
    const parsed = userData
      ? JSON.parse(userData)
      : { userId: 1, name: "Guest" };
    return {
      userId: parsed.userId || parsed.id || 1,
      name: parsed.name || "Guest",
    };
  };

  const user = getUser();

  useEffect(() => {
    fetchPosts();
  }, []);

  const fetchPosts = async () => {
    setIsLoading(true);
    try {
      const res = await axios.get("http://localhost:5015/api/PlantPost");
      setPosts(res.data);

      const initialRepliesState = {};
      res.data.forEach((post) => {
        if (post.replies && post.replies.length > 0) {
          initialRepliesState[post.id] = true;
        }
      });
      setShowReplies(initialRepliesState);
    } catch (err) {
      console.error("Failed to fetch posts:", err);
    } finally {
      setIsLoading(false);
    }
  };

  const fetchCommentsForPost = async (postId) => {
    try {
      const response = await axios.get(
        `http://localhost:5015/api/Reply/post/${postId}`
      );
      const updatedPosts = [...posts];
      const postIndex = updatedPosts.findIndex((p) => p.id === postId);

      if (postIndex !== -1) {
        updatedPosts[postIndex] = {
          ...updatedPosts[postIndex],
          replies: response.data,
        };
        setPosts(updatedPosts);
      }
    } catch (err) {
      console.error("Failed to fetch comments:", err);
    }
  };

  const handleLike = async (postId) => {
    try {
      const postIndex = posts.findIndex((p) => p.id === postId);
      if (postIndex === -1) return;

      const post = posts[postIndex];
      const likes = post.likes || [];
      const userLikeIndex = likes.findIndex(
        (like) => like.userId === user.userId
      );
      const hasLiked = userLikeIndex !== -1;

      const updatedPosts = [...posts];

      if (hasLiked) {
        updatedPosts[postIndex] = {
          ...post,
          likes: likes.filter((like) => like.userId !== user.userId),
        };
        setPosts(updatedPosts);

        await axios.delete("http://localhost:5015/api/Like", {
          data: { postId, userId: user.userId },
        });
      } else {
        const newLike = { userId: user.userId, postId };
        updatedPosts[postIndex] = {
          ...post,
          likes: [...likes, newLike],
        };
        setPosts(updatedPosts);

        await axios.post("http://localhost:5015/api/Like", {
          PostId: postId,
          UserId: user.userId,
        });
      }
    } catch (err) {
      console.error(
        "Failed to toggle like:",
        err.response?.data || err.message
      );
      fetchPosts();
    }
  };

  const handleReply = async (postId) => {
    const comment = replyInputs[postId];
    if (!comment || !comment.trim()) return;
  
    try {
      // Send to server
      const response = await axios.post("http://localhost:5015/api/Reply", {
        postId,
        userId: user.userId,
        comment,
      });
  
      if (response.data) {
        // Instead of creating our own object, use the server response
        // which should include the complete user object
        fetchCommentsForPost(postId);
        
        // Clear input
        setReplyInputs({ ...replyInputs, [postId]: "" });
        setShowReplies({ ...showReplies, [postId]: true });
      }
    } catch (err) {
      console.error("Failed to reply:", err.response?.data || err.message);
      alert("Failed to post comment. Please try again.");
    }
  };

  const handleWhatsAppShare = (post) => {
    // Create a shareable URL to the specific post
    // Replace with your actual domain when deployed
    const baseUrl = window.location.origin; // Gets current domain
    const postUrl = `${baseUrl}/post/${post.id}`; // Adjust path as needed

    // Create share text with the post URL
    const text = `Check out this plant: ${post.plantName}\n\n${
      post.description || ""
    }\n\n${postUrl}`;
    const encodedText = encodeURIComponent(text);

    // Open WhatsApp with the share text
    window.open(`https://wa.me/?text=${encodedText}`, "_blank");
  };

  const handleImageDrop = (e) => {
    e.preventDefault();
    const file = e.dataTransfer.files[0];
    if (file) handleFileSelect(file);
  };

  const handleImageUpload = (e) => {
    const file = e.target.files[0];
    if (file) handleFileSelect(file);
  };

  const handleFileSelect = (file) => {
    setNewPost({ ...newPost, imageFile: file });

    const reader = new FileReader();
    reader.onload = (e) => setPreviewImage(e.target.result);
    reader.readAsDataURL(file);
  };

  const handlePostSubmit = async () => {
    if (!newPost.plantName.trim()) {
      alert("Please enter a plant name");
      return;
    }

    setIsLoading(true);
    try {
      const formData = new FormData();
      formData.append("PlantName", newPost.plantName);
      formData.append("Description", newPost.description);
      formData.append("UserId", user.userId);
      if (newPost.imageFile) {
        formData.append("imageFile", newPost.imageFile);
      }

      const response = await axios.post(
        "http://localhost:5015/api/PlantPost/upload",
        formData,
        { headers: { "Content-Type": "multipart/form-data" } }
      );

      if (response.data) fetchPosts();
      setNewPost({ plantName: "", description: "", imageFile: null });
      setPreviewImage(null);
    } catch (err) {
      console.error(
        "Failed to create post:",
        err.response?.data || err.message
      );
    } finally {
      setIsLoading(false);
    }
  };

  const formatTimeAgo = (createdAt) => {
    try {
      const now = new Date();
      const postDate = new Date(createdAt);
      const diffMs = now - postDate;
      
      // If the post date is within the last 6 hours, just show "Just now"
      // This is a temporary fix until the timestamp issue is properly resolved
      if (diffMs < 6 * 60 * 60 * 1000 || postDate > now) {
        return "Just now";
      }
      
      const seconds = Math.floor(diffMs / 1000);
      const minutes = Math.floor(seconds / 60);
      const hours = Math.floor(minutes / 60);
      const days = Math.floor(hours / 24);
  
      if (seconds < 60) return "Just now";
      if (minutes < 60) return `${minutes} min ago`;
      if (hours < 24) return `${hours} h ago`;
      return `${days} d ago`;
    } catch (error) {
      console.error("Error formatting time:", error);
      return "Just now";
    }
  };

  const toggleReplies = (postId) => {
    const newState = !showReplies[postId];
    setShowReplies({ ...showReplies, [postId]: newState });

    // Fetch fresh comments when opening the comments section
    if (newState) {
      fetchCommentsForPost(postId);
    }
  };

  return (
    <>
      <Navbar />
      <div className="max-w-6xl mx-auto mt-6 px-4 flex flex-col lg:flex-row gap-6">
        {/* Feed Section */}
        <div className="flex-1 space-y-6">
          {isLoading && posts.length === 0 ? (
            <div className="flex justify-center items-center h-64">
              <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-green-500"></div>
            </div>
          ) : (
            [...posts].reverse().map((post) => (
              <div
                key={post.id}
                className="bg-white shadow-md rounded-xl overflow-hidden transition-all duration-300 hover:shadow-lg"
              >
                {/* Post Header */}
                <div className="p-4 flex items-center space-x-3">
                  <div className="w-10 h-10 bg-green-100 rounded-full flex items-center justify-center text-green-600">
                    {post.user?.userName?.charAt(0) || "A"}
                  </div>
                  <div>
                    <div className="font-medium">
                      {post.user?.userName || "Anonymous"}
                    </div>
                    <div className="text-xs text-gray-500 flex items-center space-x-2">
                      <span>{formatTimeAgo(post.createdAt)}</span>
                      <span>•</span>
                      <span className="flex items-center">
                        <FaLeaf className="text-green-500 mr-1" size={12} />
                        {post.plantName}
                      </span>
                    </div>
                  </div>
                </div>

                {/* Post Content */}
                <div className="px-4 py-2">
                  <p className="text-gray-800 mb-3">{post.description}</p>
                </div>

                {/* Post Image */}
                {post.imageUrl && (
                  <div className="relative w-full">
                    <img
                      src={`http://localhost:5015${post.imageUrl}`}
                      alt={post.plantName}
                      className="w-full object-cover max-h-96"
                    />
                  </div>
                )}

                {/* Post Footer */}
                <div className="p-4">
                  {/* Stats */}
                  <div className="flex items-center justify-between text-xs text-gray-500 mb-3">
                    <div>{post.likes?.length || 0} likes</div>
                    <div
                      className="cursor-pointer flex items-center"
                      onClick={() => toggleReplies(post.id)}
                    >
                      {post.replies?.length || 0} comments
                      <span className="ml-1 text-xs">
                        {showReplies[post.id] ? "▲" : "▼"}
                      </span>
                    </div>
                  </div>

                  {/* Actions */}
                  <div className="flex items-center justify-between border-t border-b border-gray-100 py-2">
                    <button
                      onClick={() => handleLike(post.id)}
                      className={`flex items-center space-x-2 px-4 py-1 rounded-full transition ${
                        post.likes?.some((like) => like.userId === user.userId)
                          ? "text-green-600 bg-green-50"
                          : "text-gray-600 hover:bg-gray-50"
                      }`}
                    >
                      <FaThumbsUp />
                      <span>Like</span>
                    </button>
                    <button
                      onClick={() => toggleReplies(post.id)}
                      className="flex items-center space-x-2 px-4 py-1 rounded-full text-gray-600 hover:bg-gray-50"
                    >
                      <FaCommentAlt />
                      <span>Comment</span>
                    </button>
                    <button
                      onClick={() => handleWhatsAppShare(post)}
                      className="flex items-center space-x-2 px-4 py-1 rounded-full text-gray-600 hover:bg-gray-50"
                    >
                      <FaWhatsapp />
                      <span>Share</span>
                    </button>
                  </div>

                  {/* Comment Section */}
                  {showReplies[post.id] && (
                    <div className="mt-3 space-y-2">
                      {post.replies?.map((reply) => (
                        <div
                          key={reply.id}
                          className="text-sm text-gray-700 flex flex-col"
                        >
                          <span className="font-medium">
                            {reply.user?.userName || "User"}
                          </span>
                          <span>{reply.comment}</span>
                        </div>
                      ))}
                      <div className="flex mt-2">
                        <input
                          type="text"
                          className="flex-1 border rounded-l-full px-4 py-1 text-sm focus:outline-none"
                          placeholder="Write a comment..."
                          value={replyInputs[post.id] || ""}
                          onChange={(e) =>
                            setReplyInputs({
                              ...replyInputs,
                              [post.id]: e.target.value,
                            })
                          }
                        />
                        <button
                          onClick={() => handleReply(post.id)}
                          className="bg-green-500 text-white px-3 rounded-r-full text-sm"
                        >
                          <FaPaperPlane />
                        </button>
                      </div>
                    </div>
                  )}
                </div>
              </div>
            ))
          )}
        </div>

        {/* Upload New Post Section */}
        <div className="w-full lg:w-1/3 bg-white rounded-xl shadow-md p-6">
          <div
            onDrop={handleImageDrop}
            onDragOver={(e) => e.preventDefault()}
            className="border-2 border-dashed border-gray-300 rounded-xl h-40 flex flex-col items-center justify-center text-gray-400 cursor-pointer hover:border-green-500 transition mb-4"
            onClick={() => document.getElementById("image-upload").click()}
          >
            {previewImage ? (
              <img
                src={previewImage}
                alt="Preview"
                className="h-full object-cover rounded-xl"
              />
            ) : (
              <>
                <FaCamera size={30} />
                <span className="mt-2 text-sm">
                  Drag & Drop or Click to Upload
                </span>
              </>
            )}
            <input
              id="image-upload"
              type="file"
              onChange={handleImageUpload}
              className="hidden"
              accept="image/*"
            />
          </div>

          <input
            type="text"
            placeholder="Plant Name"
            className="w-full border p-2 rounded-lg mb-2"
            value={newPost.plantName}
            onChange={(e) =>
              setNewPost({ ...newPost, plantName: e.target.value })
            }
          />

          <textarea
            placeholder="Description"
            className="w-full border p-2 rounded-lg mb-2"
            rows="3"
            value={newPost.description}
            onChange={(e) =>
              setNewPost({ ...newPost, description: e.target.value })
            }
          ></textarea>

          <button
            onClick={handlePostSubmit}
            className="w-full bg-green-500 text-white p-2 rounded-lg hover:bg-green-600 transition"
          >
            Upload Your Queries
          </button>
        </div>
      </div>
    </>
  );
};

export default Feed;
