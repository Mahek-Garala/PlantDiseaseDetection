import React, { useEffect, useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import axios from "axios";
import Navbar from "./Navbar";
import Footer from "./Footer";

const RecommendationPage = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const [recommendation, setRecommendation] = useState("");
    const [loading, setLoading] = useState(true);

    // Extract disease from URL query params
    const queryParams = new URLSearchParams(location.search);
    const disease = queryParams.get("disease");

    useEffect(() => {
        if (!disease) {
            setRecommendation("No disease specified.");
            setLoading(false);
            return;
        }

        axios
            .get(`http://127.0.0.1:8000/recommend?disease=${encodeURIComponent(disease)}`)
            .then((response) => {
                if (response.data && response.data.recommendation) {
                    setRecommendation(response.data.recommendation);
                } else {
                    setRecommendation("No specific recommendation available.");
                }
            })
            .catch((error) => {
                console.error("Error fetching recommendations:", error.response ? error.response.data : error.message);
                setRecommendation("Failed to fetch recommendations.");
            })
            .finally(() => setLoading(false));
    }, [disease]);

    return (
        <div className="flex flex-col min-h-screen">
            <Navbar />
            <main className="flex-1 flex flex-col items-center py-10 px-4">
                <h2 className="text-2xl font-semibold">Cure Recommendations</h2>
                {loading ? (
                    <p className="mt-4">Loading recommendations...</p>
                ) : (
                    <div className="mt-4 text-lg text-green-600">
                        <p><strong>Disease:</strong> {disease ? disease.replace(/_/g, " ") : "Unknown"}</p>
                        <p><strong>Recommendation:</strong></p>
                        <div className="mt-2 p-4 bg-gray-100 border rounded text-left whitespace-pre-line">
                            {recommendation}
                        </div>
                    </div>
                )}
            </main>
            <Footer />
        </div>
    );
};

export default RecommendationPage;