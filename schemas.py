from pydantic import BaseModel
from typing import Optional

class StudentInput(BaseModel):
    # Демография
    gender_enc: float = 0
    region_enc: float = 0
    highest_education_enc: float = 0
    imd_band_enc: float = 0
    age_band_enc: float = 0
    disability_enc: float = 0
    # История обучения
    num_of_prev_attempts: float = 0
    studied_credits: float = 60
    # Оценки (Блок А — уровень знаний)
    avg_score: float = 0
    max_score: float = 0
    min_score: float = 0
    std_score: float = 0
    submission_count: float = 0
    submission_rate: float = 0
    score_trend: float = 0
    # Динамика активности по кварталам
    q1_clicks: float = 0
    q2_clicks: float = 0
    q3_clicks: float = 0
    q4_clicks: float = 0
    early_vs_late: float = 0
    # Регистрация
    early_registration: float = 0
    unregistered: float = 0
    # Стиль восприятия (Блок Б)
    total_clicks: float = 0
    ratio_video: float = 0
    ratio_oucontent: float = 0
    ratio_quiz: float = 0
    ratio_forumng: float = 0
    ratio_resource: float = 0
    ratio_page: float = 0
    dominant_style_enc: float = 0
    # Глубина взаимодействия
    unique_resources: float = 0
    total_sessions: float = 0
    avg_clicks_per_res: float = 0
    revisit_rate: float = 0
    study_regularity: float = 0

class PredictionOutput(BaseModel):
    prediction: str
    probabilities: dict
    top_features: list
    learning_style: str
    recommendation: str