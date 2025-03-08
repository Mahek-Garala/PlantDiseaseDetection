import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
//import { Button } from "@/components/ui/button";
//import { Input } from "@/components/ui/input";
//import { Card, CardContent } from "@/components/ui/card";
import { Loader2 } from "lucide-react";
import Navbar from "./Navbar";
import Footer from "./Footer";

const HomePage = () => {
    const [image, setImage] = useState(null);
    const [preview, setPreview] = useState(null);
    const [loading, setLoading] = useState(false);
    const [result, setResult] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (!token) {
            navigate("/auth"); // Redirect to login if not authenticated
        }
    }, [navigate]);

    const handleImageChange = (e) => {
        const file = e.target.files[0];
        setImage(file);
        setPreview(URL.createObjectURL(file));
    };

    const handleUpload = async () => {
        if (!image) return;
        setLoading(true);
        const formData = new FormData();
        formData.append("file", image);
        formData.append("upload_preset", "your_upload_preset");

        try {
            const cloudinaryResponse = await axios.post(
                "https://api.cloudinary.com/v1_1/your_cloud_name/image/upload",
                formData
            );
            const imageUrl = cloudinaryResponse.data.secure_url;

            const detectionResponse = await axios.post("/api/detect", { imageUrl });
            setResult(detectionResponse.data);
        } catch (error) {
            console.error("Error uploading or detecting:", error);
        }
        setLoading(false);
    };

    return (
        <div className="flex flex-col min-h-screen">
            <Navbar />

            {/* Hero Section */}
            <header className="text-center py-12 bg-green-100">
                <h2 className="text-3xl font-semibold">Detect Plant Diseases Easily</h2>
                <p className="mt-2 text-gray-600">Upload an image and get instant disease diagnosis</p>
            </header>

            {/* Image Upload & Detection */}
            <main className="flex-1 flex flex-col items-center py-10 px-4">
                <input type="file" accept="image/*" onChange={handleImageChange} className="mt-2 p-2 border rounded" />
                <button onClick={handleUpload} className="mt-4 bg-blue-500 text-white px-4 py-2 rounded" disabled={loading}>
                    {loading ? <span className="animate-spin">🔄</span> : "Detect Disease"}
                </button>
                <div className="w-full max-w-md p-6 border rounded shadow">
                    <div className="flex flex-col items-center">
                        {/* Content here */}
                    </div>
                </div>


            </main>

            <Footer />
        </div>
    );
};

export default HomePage;
