import React, { useEffect, useState } from "react";
import axios from "axios";
import Footer from "./Footer";
import Navbar from "./Navbar";

const Profile = () => {
    const [profile, setProfile] = useState({ userName: "", email: "" });
    const [updatedProfile, setUpdatedProfile] = useState({ userName: "", email: "" });
    const [editing, setEditing] = useState(false);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const token = localStorage.getItem("token");
                if (!token) {
                    setError("No token found. Please login again.");
                    return;
                }

                const response = await axios.get("http://localhost:5015/api/auth/profile", {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                console.log("Profile API Response:", response.data);
                setProfile(response.data);
                setUpdatedProfile(response.data);
                setLoading(false);
            } catch (err) {
                console.error("Profile Fetch Error:", err);
                setError("Failed to fetch profile.");
                setLoading(false);
            }
        };

        fetchProfile();
    }, []);

    const handleUpdateProfile = async () => {
        try {
            const token = localStorage.getItem("token");

            if (!token) {
                setError("Authentication failed. Please log in again.");
                return;
            }

            const response = await axios.put(
                "http://localhost:5015/api/auth/profile",
                {
                    userName: updatedProfile.userName.trim(),
                    email: updatedProfile.email.trim(),
                },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                }
            );

            console.log("✅ Profile Updated:", response.data);
            setProfile(updatedProfile);
            setSuccess("Profile updated successfully!");
            setEditing(false);

            setTimeout(() => setSuccess(null), 2000);
        } catch (error) {
            console.error("❌ Profile Update Error:", error);
            setError("Failed to update profile. Please try again.");
        }
    };

    if (loading) return <p className="text-center text-gray-600 text-lg">Loading...</p>;
    if (error) return <p className="text-center text-red-500 text-lg">{error}</p>;

    return (
        <>
            <Navbar />

            {success && (
                <div className="fixed top-10 left-1/2 transform -translate-x-1/2 bg-green-500 text-white py-3 px-6 rounded-lg shadow-lg text-center">
                    {success}
                </div>
            )}

            <div className="flex justify-center items-center min-h-screen bg-gradient-to-br from-green-200 to-yellow-100">
                <div className="bg-white shadow-lg rounded-2xl p-8 max-w-sm w-full text-center transform transition duration-300 hover:scale-105">
                    <h2 className="text-2xl font-bold text-gray-800 mb-4">User Profile</h2>

                    <div className="bg-gray-100 p-4 rounded-lg shadow-inner">
                        {editing ? (
                            <>
                                <input
                                    type="text"
                                    className="w-full p-2 rounded-md border border-gray-300 mb-2"
                                    value={updatedProfile.userName}
                                    onChange={(e) => setUpdatedProfile({ ...updatedProfile, userName: e.target.value })}
                                />
                                <input
                                    type="email"
                                    className="w-full p-2 rounded-md border border-gray-300"
                                    value={updatedProfile.email}
                                    onChange={(e) => setUpdatedProfile({ ...updatedProfile, email: e.target.value })}
                                />
                            </>
                        ) : (
                            <>
                                <p className="text-lg text-gray-700">
                                    <strong className="text-gray-900">Username:</strong> {profile.userName}
                                </p>
                                <p className="text-lg text-gray-700 mt-2">
                                    <strong className="text-gray-900">Email:</strong> {profile.email}
                                </p>
                            </>
                        )}
                    </div>

                    {editing ? (
                        <div className="mt-4 space-x-2">
                            <button
                                onClick={handleUpdateProfile}
                                className="px-4 py-2 bg-green-600 text-white font-semibold rounded-lg hover:bg-green-700 transition"
                            >
                                Save
                            </button>
                            <button
                                onClick={() => setEditing(false)}
                                className="px-4 py-2 bg-gray-500 text-white font-semibold rounded-lg hover:bg-gray-600 transition"
                            >
                                Cancel
                            </button>
                        </div>
                    ) : (
                        <button
                            onClick={() => setEditing(true)}
                            className="mt-6 px-5 py-2 bg-green-600 text-white font-semibold rounded-lg hover:bg-green-700 transition"
                        >
                            Edit Profile
                        </button>
                    )}
                </div>
            </div>
            <Footer />
        </>
    );
};

export default Profile;
