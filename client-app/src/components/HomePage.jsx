import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import Navbar from "./Navbar";
import Footer from "./Footer";

const HomePage = () => {
    const [image, setImage] = useState(null);
    const [preview, setPreview] = useState(null);
    const [loading, setLoading] = useState(false);
    const [disease, setDisease] = useState(null);
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

        try {
            // Upload to Flask API
            const response = await axios.post("http://localhost:8080/predict", formData, {
                headers: { "Content-Type": "multipart/form-data" }
            });
            setDisease(response.data.disease);
        } catch (error) {
            console.error("Error in prediction:", error);
        }
        setLoading(false);
    };

    // Function to navigate to RecommendationPage with disease
    const handleGetRecommendations = () => {
        if (disease) {
            navigate(`/recommendations?disease=${encodeURIComponent(disease)}`);
        } else {
            alert("Please detect a disease first!");
        }
    };

    return (
        <div className="flex flex-col min-h-screen">
            <Navbar />
            <header className="text-center py-12 bg-green-100">
                <h2 className="text-3xl font-semibold">Detect Plant Diseases Easily</h2>
                <p className="mt-2 text-gray-600">Upload an image and get instant disease diagnosis</p>
            </header>
            <main className="flex-1 flex flex-col items-center py-10 px-4">
                <input type="file" accept="image/*" onChange={handleImageChange} className="mt-2 p-2 border rounded" />
                {preview && <img src={preview} alt="Preview" className="mt-4 w-64 h-64 object-cover border" />}
                <button onClick={handleUpload} className="mt-4 bg-green-600 text-white px-4 py-2 rounded" disabled={loading}>
                    {loading ? "Processing..." : "Detect Disease"}
                </button>
                {disease && (
                    <div className="mt-6 p-4 border rounded bg-white shadow-md">
                        <h3 className="text-xl font-semibold">Detected Disease</h3>
                        <p className="text-lg text-red-600 font-bold">{disease.replace(/_/g, " ")}</p>
                        <button onClick={handleGetRecommendations} className="mt-4 bg-green-500 text-white px-4 py-2 rounded">
                            Get Cure Recommendations
                        </button>
                    </div>
                )}
            </main>
            <Footer />
        </div>
    );
};

export default HomePage;
