# import google.generativeai as genai
# from fastapi import FastAPI, Query
# from fastapi.middleware.cors import CORSMiddleware


# # # Configure API Key
# # genai.configure(api_key="AIzaSyAmmQmZMqMagDsFXStjSi-UcX6wbajyhLs")

# # # Define Model
# # model = genai.GenerativeModel("gemini-1.5-pro-latest")

# # # Initialize FastAPI
# # app = FastAPI()

# # app.add_middleware(
# #     CORSMiddleware,
# #     allow_origins=["*"],  # Adjust for security (e.g., ["http://localhost:3000"])
# #     allow_credentials=True,
# #     allow_methods=["*"],
# #     allow_headers=["*"],
# # )

# # @app.get("/recommend")
# # def get_recommendation(disease: str = Query(..., title="Plant Disease")):
# #     """
# #     Given a plant disease name, returns a recommended treatment and a YouTube video link (if available).
# #     """
# #     prompt = f"What is the best treatment for {disease} in plants? Provide a YouTube video link if available."
    
# #     try:
# #         response = model.generate_content(prompt)
# #         recommendation = response.text.strip()
        
# #         return {"disease": disease, "recommendation": recommendation}
# #     except Exception as e:
# #         return {"error": str(e)}

# # # #python -m uvicorn recommend:app --reload

# import google.generativeai as genai
# from fastapi import FastAPI, Query
# from fastapi.middleware.cors import CORSMiddleware

# # Configure API Key
# genai.configure(api_key="AIzaSyBb-PAjtBT0GFsupf_0DDI5jksXWK1zvYs")
# # genai.configure(api_key="AIzaSyAmmQmZMqMagDsFXStjSi-UcX6wbajyhLs")

# # Define Model
# model = genai.GenerativeModel("gemini-1.5-pro-latest")

# # Initialize FastAPI
# app = FastAPI()

# app.add_middleware(
#     CORSMiddleware,
#     allow_origins=["*"],  # Adjust for security (e.g., ["http://localhost:3000"])
#     allow_credentials=True,
#     allow_methods=["*"],
#     allow_headers=["*"],
# )

# @app.get("/recommend")
# def get_recommendation(
#     disease: str = Query(..., title="Plant Disease"),
#     language: str = Query("english", title="Language")
# ):
#     """
#     Given a plant disease name and language, returns a recommended treatment and a YouTube video link (if available).
#     """
#     prompt = (
#         f"What is the best treatment for {disease} in plants? Provide a YouTube video link if available. "
#         f"Respond in {language}."
#     )
    
#     try:
#         response = model.generate_content(prompt)

#         recommendation = response.text.strip()

#         print(recommendation)
        
        
#         return {"disease": disease, "recommendation": recommendation, "language": language}
#     except Exception as e:
#         return {"error": str(e)}

# # # Run with: python -m uvicorn recommend:app --reload
# # import google.generativeai as genai
# # from fastapi import FastAPI, Query
# # from fastapi.middleware.cors import CORSMiddleware
# # import random
# # # Configure API Key
# # genai.configure(api_key="AIzaSyAmmQmZMqMagDsFXStjSi-UcX6wbajyhLs")

# # # Define Model
# # model = genai.GenerativeModel("gemini-1.5-pro-latest")

# # # Initialize FastAPI
# # app = FastAPI()

# # app.add_middleware(
# #     CORSMiddleware,
# #     allow_origins=["*"],
# #     allow_credentials=True,
# #     allow_methods=["*"],
# #     allow_headers=["*"],
# # )

# # # Dummy expert data per disease
# # disease_experts = {
# #     "powdery mildew": [
# #         {"name": "Dr. Smith", "phone": "+1800111222", "video": "https://meet.jit.si/SmithMildewExpert"},
# #         {"name": "Dr. Patel", "phone": "+1800333444", "video": "https://meet.jit.si/PatelMildewGuru"}
# #     ],
# #     "Tomato___Early_blight": [
# #         {"name": "Dr. Lee", "phone": "+1800555666", "video": "https://meet.jit.si/LeeBlightExpert"},
# #         {"name": "Dr. Kumar", "phone": "+1800777888", "video": "https://meet.jit.si/KumarBlightPro"}
# #     ],
# #     "rust": [
# #         {"name": "Dr. Green", "phone": "+1800999000", "video": "https://meet.jit.si/GreenRustFixer"}
# #     ]
# # }

# # # Get an expert based on disease
# # def get_expert(disease):
# #     experts = disease_experts.get(disease.lower(), [{"name": "General Expert", "phone": "+18001234567", "video": "https://meet.jit.si/GeneralPlantHelp"}])
# #     return random.choice(experts)

# # @app.get("/recommend")
# # def get_recommendation(
# #     disease: str = Query(..., title="Plant Disease"),
# #     language: str = Query("english", title="Language")
# # ):
# #     """
# #     Returns the recommendation for the disease and assigns an expert for call/video.
# #     """
# #     # Generate a recommendation prompt
# #     prompt = f"What is the best treatment for {disease} in plants? Provide a YouTube video link if available. Respond in {language}."

# #     try:
# #         # Get recommendation from Gemini
# #         response = model.generate_content(prompt)
# #         recommendation = f"For {disease}, a general treatment suggestion is to ensure proper watering and sunlight."

# #        # recommendation = response.text.strip()

# #         # Fetch a suitable expert
# #         expert = get_expert(disease)

# #         return {
# #             "disease": disease,
# #             "recommendation": recommendation,
# #             "language": language,
# #             "phone_call_link": f"tel:{expert['phone']}",
# #             "video_call_link": expert["video"],
# #             "expert_name": expert["name"]
# #         }
# #     except Exception as e:
# #         return {"error": str(e)}


# main.py (or whatever your file is named)

from fastapi import FastAPI, Query
from fastapi.middleware.cors import CORSMiddleware
import google.generativeai as genai

# 🔐 Configure Gemini API
# genai.configure(api_key="AIzaSyBb-PAjtBT0GFsupf_0DDI5jksXWK1zvYs")
genai.configure(api_key="AIzaSyAmmQmZMqMagDsFXStjSi-UcX6wbajyhLs")      
# 🌱 Create app instance
app = FastAPI()

# 🌍 Enable CORS for frontend to connect
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # Replace with ["http://localhost:3000"] for better security
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# ✨ Define Gemini model
model = genai.GenerativeModel("gemini-1.5-pro-latest")

@app.get("/recommend")
def get_recommendation(
    disease: str = Query(..., title="Plant Disease"),
    language: str = Query("english", title="Language")
):
    prompt = (
        f"What is the best treatment for {disease} in plants? Provide a YouTube video link if available. "
        f"Respond in {language}."
    )

    print("🚀 Prompt:", prompt)

    try:
        response = model.generate_content(prompt)
        print("✅ Gemini response received")
        print("🔧 Raw response:", response)

        recommendation = response.text.strip()
        return {
            "disease": disease,
            "recommendation": recommendation,
            "language": language
        }
    except Exception as e:
        print("❌ Error calling Gemini API:", str(e))
        return {"error": str(e)}
