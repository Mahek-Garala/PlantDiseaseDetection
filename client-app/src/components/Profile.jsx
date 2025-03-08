import React from "react";

const UserProfile = () => {
    return (
        <div className="flex justify-center items-center min-h-screen bg-green-100">
            <div className="w-full max-w-2xl bg-white shadow-lg rounded-lg overflow-hidden">
                {/* Profile Header */}
                <div className="relative bg-green-700 h-40">
                    <div className="absolute -bottom-12 left-1/2 transform -translate-x-1/2">
                        <img
                            src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-profiles/avatar-1.webp"
                            alt="Profile"
                            className="w-24 h-24 rounded-full border-4 border-white shadow-md"
                        />
                    </div>
                </div>

                {/* User Info */}
                <div className="text-center mt-14 p-4">
                    <h2 className="text-2xl font-semibold text-gray-800">Andy Horwitz</h2>
                    <p className="text-gray-500">Farmer | Agricultural Expert</p>
                    <p className="text-gray-600 mt-2">New York, USA</p>

                    <button className="mt-3 px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition">
                        Edit Profile
                    </button>
                </div>

                {/* About Section */}
                <div className="p-6">
                    <h3 className="text-xl font-semibold text-gray-800">About</h3>
                    <p className="text-gray-600 mt-2">
                        Experienced farmer specializing in organic crops and modern agricultural techniques. Passionate about sustainable farming and community support.
                    </p>
                </div>
            </div>
        </div>
    );
};

export default UserProfile;
