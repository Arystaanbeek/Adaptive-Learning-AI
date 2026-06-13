# AdaptiveLearn AI

## Overview

AdaptiveLearn AI is an intelligent adaptive learning platform developed as part of a master's thesis research project.

The system uses machine learning techniques to predict student academic performance and personalize educational content according to individual learning styles and behavior patterns.

The project combines Learning Analytics, Educational Data Mining, and Artificial Intelligence technologies to improve the effectiveness of digital education.

---

## Research Topic

**Development and Testing of an AI Model for Adaptive Learning that Adapts to Students' Knowledge Level and Learning Style**

Master's Thesis

Kazakh National Research Technical University named after K.I. Satpayev

---

## Main Features

* Student performance prediction using Machine Learning
* Automatic learning style detection
* Personalized educational recommendations
* Adaptive content delivery
* Learning progress tracking
* User authentication and authorization
* Course management system
* Educational analytics dashboard

---

## Learning Styles Supported

The platform identifies the dominant learning style of each student:

* Visual
* Reading
* Interactive
* Research
* Social

Based on the detected style, the system recommends the most appropriate educational materials and learning activities.

---

## Technology Stack

### Backend

* ASP.NET Core Web API
* Entity Framework Core
* SQLite
* JWT Authentication

### AI Service

* FastAPI
* Scikit-learn
* Pandas
* NumPy
* Random Forest
* SHAP

### Frontend

* HTML
* CSS
* JavaScript
* Bootstrap

### Dataset

* Open University Learning Analytics Dataset (OULAD)

---

## System Architecture

```text
Web Client
     |
ASP.NET Core API
     |
Database (SQLite)
     |
FastAPI AI Service
     |
Random Forest Model
```

---

## Machine Learning Model

The platform uses the Random Forest algorithm to predict student academic performance.

Input features include:

* Average assessment score
* Submission rate
* Student activity
* Learning engagement
* Resource usage statistics
* Learning style indicators

The model output is a prediction of student academic performance and personalized recommendations.

---

## Project Structure

```text
AdaptiveLearning.API/
│
├── Controllers/
├── Models/
├── Services/
├── Data/
├── Migrations/
├── wwwroot/
├── Program.cs
├── appsettings.json
└── AdaptiveLearning.API.sln

ai_service/
│
├── main.py
├── model.py
├── schemas.py
├── requirements.txt
└── trained_model.pkl
```

---

## Installation

### ASP.NET Core API

```bash
dotnet restore
dotnet run
```

### AI Service

```bash
pip install -r requirements.txt
uvicorn main:app --reload
```

---

## Research Contributions

* Development of an adaptive learning AI model
* Automatic learning style detection mechanism
* Student performance prediction system
* Personalized recommendation engine
* Integration of ASP.NET Core and FastAPI services

---

## Results

The experimental evaluation demonstrated the effectiveness of the proposed approach for:

* Predicting academic performance
* Identifying learning styles
* Personalizing educational content
* Improving adaptive learning experiences

---

## Author

Muhammed Arystanbek

Master's Program: Management of Information Systems

Satbayev University

2026
