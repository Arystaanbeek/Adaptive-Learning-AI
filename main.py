from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from schemas import StudentInput, PredictionOutput
from model import adaptive_model
import uvicorn

app = FastAPI(
    title="Adaptive Learning AI Service",
    description="AI-микросервис для предсказания успеваемости и стиля восприятия студентов",
    version="1.0.0"
)

# Разрешаем запросы от .NET и React
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5000", "http://localhost:3000", "*"],
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/")
def root():
    return {"status": "ok", "service": "Adaptive Learning AI", "version": "1.0.0"}

@app.get("/health")
def health():
    return {"status": "healthy", "model_loaded": adaptive_model.model is not None}

@app.post("/predict", response_model=PredictionOutput)
def predict(student: StudentInput):
    try:
        features = student.model_dump()
        result = adaptive_model.predict(features)
        return result
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

@app.get("/model-info")
def model_info():
    model = adaptive_model.model
    return {
        "algorithm": type(model).__name__,
        "n_estimators": model.n_estimators,
        "criterion": model.criterion,
        "classes": list(model.classes_),
        "n_features": model.n_features_in_,
    }

if __name__ == "__main__":
    uvicorn.run("main:app", host="0.0.0.0", port=8000, reload=True)