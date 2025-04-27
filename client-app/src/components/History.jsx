import React, { useEffect, useState } from "react";
import axios from "axios";
import Navbar from "./Navbar";
import { useNavigate } from "react-router-dom";
import { Clock, Leaf, AlertCircle } from "lucide-react";

const History = () => {
  const [history, setHistory] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    fetchHistory();
  }, []);

  const fetchHistory = async () => {
    try {
      const token = localStorage.getItem("token");
      if (!token) {
        setError("You need to log in to view your history");
        setLoading(false);
        return;
      }

      const response = await axios.get(
        "http://localhost:5015/api/plant/history",
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );
      setHistory(response.data);
    } catch (error) {
      console.error("Error fetching history:", error);
      setError("Failed to load your history. Please try again later.");
    } finally {
      setLoading(false);
    }
  };

  const handleRecommendation = (diseaseName) => {
    if (diseaseName) {
      navigate(`/recommendations?disease=${encodeURIComponent(diseaseName)}`);
    } else {
      alert("No disease detected for this plant!");
    }
  };

  if (loading) {
    return (
      <>
        <Navbar />
        <div className="flex flex-col items-center justify-center min-h-screen bg-gray-50">
          <div className="w-16 h-16 border-4 border-green-500 border-t-transparent rounded-full animate-spin"></div>
          <p className="mt-4 text-gray-600 font-medium">
            Loading your plant history...
          </p>
        </div>
      </>
    );
  }

  if (error) {
    return (
      <>
        <Navbar />
        <div className="flex flex-col items-center justify-center min-h-screen bg-gray-50">
          <AlertCircle size={48} className="text-red-500" />
          <p className="mt-4 text-gray-700 font-medium">{error}</p>
          <button
            onClick={() => window.location.reload()}
            className="mt-4 px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition"
          >
            Try Again
          </button>
        </div>
      </>
    );
  }

  return (
    <>
      <Navbar />
      <div className="max-w-6xl mx-auto p-6">
        <div className="mb-8 text-center">
          <h2 className="text-3xl font-bold text-green-800">
            My Plant History
          </h2>
          <p className="text-gray-600 mt-2">
            View all your previously analyzed plants
          </p>
        </div>

        {history.length === 0 ? (
          <div className="bg-white rounded-lg shadow-md p-8 text-center">
            <Leaf size={64} className="text-green-500 mx-auto mb-4" />
            <h3 className="text-xl font-semibold text-gray-800 mb-2">
              No plants analyzed yet
            </h3>
            <p className="text-gray-600 mb-6">
              Upload your first plant image to get started!
            </p>
            <button
              onClick={() => navigate("/home")}
              className="px-6 py-3 bg-green-600 text-white rounded-lg hover:bg-green-700 transition"
            >
              Upload a Plant
            </button>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            {history.map((item) => (
              <div
                key={item.imageId}
                className="bg-white border rounded-xl shadow-md overflow-hidden hover:shadow-lg transition"
              >
                {/* Image container with consistent aspect ratio */}
                <div className="relative pb-2/3 h-60">
                  <img
                    src={`http://localhost:5015${item.imageUrl}`}
                    alt="Plant"
                    className="absolute w-full h-full object-cover"
                    onError={(e) => {
                      e.target.onerror = null;
                      e.target.src = "/placeholder-plant.jpg";
                    }}
                  />
                </div>

                <div className="p-5">
                  <div className="flex items-center mb-3">
                    <div
                      className={`w-3 h-3 rounded-full mr-2 ${
                        item.diseaseName ? "bg-yellow-500" : "bg-green-500"
                      }`}
                    ></div>
                    <h3 className="font-semibold text-lg text-gray-800 truncate">
                      {item.diseaseName || "Healthy Plant"}
                    </h3>
                  </div>

                  <div className="flex items-center text-gray-500 text-sm mb-4">
                    <Clock size={22} className="mr-1" />
                    <span>
                      {new Date(item.uploadedAt).toLocaleDateString()} at{" "}
                      {new Date(item.uploadedAt).toLocaleTimeString([], {
                        hour: "2-digit",
                        minute: "2-digit",
                      })}
                    </span>
                  </div>

                  <button
                    onClick={() => handleRecommendation(item.diseaseName)}
                    className="w-full px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition flex items-center justify-center"
                    disabled={!item.diseaseName}
                  >
                    <Leaf size={18} className="mr-2" />
                    {item.diseaseName
                      ? "Get Treatment Advice"
                      : "No Treatment Needed"}
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </>
  );
};

export default History;
