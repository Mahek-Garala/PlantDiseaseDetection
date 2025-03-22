import google.generativeai as genai
from fastapi import FastAPI, Query
from fastapi.middleware.cors import CORSMiddleware


# Configure API Key
genai.configure(api_key="AIzaSyBb-PAjtBT0GFsupf_0DDI5jksXWK1zvYs")

# Define Model
model = genai.GenerativeModel("gemini-1.5-pro-latest")

# Initialize FastAPI
app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # Adjust for security (e.g., ["http://localhost:3000"])
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/recommend")
def get_recommendation(disease: str = Query(..., title="Plant Disease")):
    """
    Given a plant disease name, returns a recommended treatment and a YouTube video link (if available).
    """
    prompt = f"What is the best treatment for {disease} in plants? Provide a YouTube video link if available."
    
    try:
        response = model.generate_content(prompt)
        recommendation = response.text.strip()
        
        return {"disease": disease, "recommendation": recommendation}
    except Exception as e:
        return {"error": str(e)}

#python -m uvicorn recommend:app --reload