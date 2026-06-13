import pickle
import numpy as np
from pathlib import Path

# Список признаков строго в том же порядке что при обучении
FEATURE_COLS = [
    "gender_enc", "region_enc", "highest_education_enc",
    "imd_band_enc", "age_band_enc", "disability_enc",
    "num_of_prev_attempts", "studied_credits",
    "avg_score", "max_score", "min_score", "std_score",
    "submission_count", "submission_rate", "score_trend",
    "q1_clicks", "q2_clicks", "q3_clicks", "q4_clicks", "early_vs_late",
    "early_registration", "unregistered",
    "total_clicks",
    "ratio_video", "ratio_oucontent", "ratio_quiz",
    "ratio_forumng", "ratio_resource", "ratio_page",
    "dominant_style_enc",
    "unique_resources", "total_sessions",
    "avg_clicks_per_res", "revisit_rate", "study_regularity",
]

STYLE_MAP = {
    0: "interactive",
    1: "reading",
    2: "research",
    3: "social",
    4: "visual",
}

RECOMMENDATIONS = {
    "Pass":        "Студент успешно справляется. Рекомендуем усложнить материал.",
    "Distinction": "Отличный результат! Рекомендуем дополнительные углублённые задания.",
    "Fail":        "Требуется поддержка. Рекомендуем упростить материал и усилить обратную связь.",
    "Withdrawn":   "Высокий риск отчисления. Срочно требуется вмешательство куратора.",
}

class AdaptiveModel:
    def __init__(self):
        self.model = None
        self.load()

    def load(self):
        model_path = Path(__file__).parent / "oulad_adaptive_model.pkl"
        if not model_path.exists():
            raise FileNotFoundError(f"Модель не найдена: {model_path}")
        with open(model_path, "rb") as f:
            self.model = pickle.load(f)
        print(f"Модель загружена. Классы: {self.model.classes_}")

    def predict(self, features: dict) -> dict:
        # Собираем вектор в нужном порядке
        X = np.array([[features.get(f, 0) for f in FEATURE_COLS]])
        X = np.nan_to_num(X, nan=0, posinf=0, neginf=0)

        # Предсказание
        prediction = self.model.predict(X)[0]
        proba = self.model.predict_proba(X)[0]
        classes = self.model.classes_

        # Вероятности по классам
        probabilities = {cls: round(float(p), 4) for cls, p in zip(classes, proba)}

        # Топ-5 важных признаков
        importances = self.model.feature_importances_
        top_idx = np.argsort(importances)[::-1][:5]
        top_features = [
            {"feature": FEATURE_COLS[i], "importance": round(float(importances[i]), 4)}
            for i in top_idx
        ]

        # Стиль восприятия
        style_enc = int(round(float(features.get("dominant_style_enc", 0))))
        learning_style = STYLE_MAP.get(style_enc, "reading")

        print(
            f"DEBUG: prediction={prediction}, style_enc={style_enc}, learning_style={learning_style}, top_features={top_features}")

        return {
            "prediction": prediction,
            "probabilities": probabilities,
            "top_features": top_features,
            "learning_style": learning_style,
            "recommendation": RECOMMENDATIONS.get(prediction, ""),
        }

# Создаём один экземпляр при загрузке модуля
adaptive_model = AdaptiveModel()