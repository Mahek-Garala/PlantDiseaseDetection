import React from "react";
import { Link, useNavigate } from "react-router-dom";

const Navbar = () => {
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem("token");
        navigate("/auth");
    };

    return (
        <nav className="bg-green-600 text-white py-4 px-6 flex justify-between items-center shadow-md">
            {/* Clicking "FarmEase" redirects to homepage */}
            <h1
                className="text-3xl font-bold cursor-pointer hover:text-gray-200 transition"
                onClick={() => navigate("/home")}
            >
                FarmEase
            </h1>

            <div className="flex space-x-4">
                <Link to="/profile">
                    <button className="px-4 py-2 border border-white rounded-lg hover:bg-white hover:text-green-600 transition">
                        Profile
                    </button>
                </Link>

                {/* Uncomment if History is needed */}
                {/* <Link to="/history">
                    <button className="px-4 py-2 border border-white rounded-lg hover:bg-white hover:text-green-600 transition">
                        History
                    </button>
                </Link> */}

                <button
                    className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition"
                    onClick={handleLogout}
                >
                    Logout
                </button>
            </div>
        </nav>
    );
};

export default Navbar;
