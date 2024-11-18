using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouteManager : MonoBehaviour
{
    public TextMeshProUGUI lapText; // UI элемент для отображения текущего круга
    public TextMeshProUGUI timeText; // UI элемент для отображения времени
    public TextMeshProUGUI pointText; // UI элемент для отображения количества пройденных точек

    public int totalLaps = 2; // Общее количество кругов
    private int currentLap = 1;
    private float lapTime = 0f;
    private int totalPoints;
    private int pointsPassed = 0;
    private bool isRacing = false;

    public List<Collider> routePoints = new List<Collider>();
    private Collider startLine;
    private Collider finishLine;

    void Start()
    {
        // Найти все точки маршрута
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        totalPoints = points.Length;
        foreach (GameObject point in points)
        {
            routePoints.Add(point.GetComponent<Collider>());
        }

        // Найти триггеры начала и финиша
        startLine = GameObject.FindGameObjectWithTag("StartLine").GetComponent<Collider>();
        finishLine = GameObject.FindGameObjectWithTag("FinishLine").GetComponent<Collider>();
        finishLine.enabled = false; // Отключить триггер финиша до прохождения всех точек

        UpdateUI();
    }

    void Update()
    {
        if (isRacing)
        {
            lapTime += Time.deltaTime; // Считаем время круга
            UpdateUI();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StartLine") && !isRacing)
        {
            // Начало гонки
            isRacing = true;
            startLine.enabled = false; // Отключить триггер начала после прохождения
        }
        else if (other.CompareTag("Point"))
        {
            if (routePoints.Contains(other))
            {
                routePoints.Remove(other); // Удаляем точку из списка непройденных
                pointsPassed++;
                if (routePoints.Count == 0)
                {
                    finishLine.enabled = true; // Включаем финиш, если все точки пройдены
                }
            }
        }
        else if (other.CompareTag("FinishLine") && isRacing && routePoints.Count == 0)
        {
            // Конец круга
            currentLap++;
            if (currentLap > totalLaps)
            {
                isRacing = false; // Гонка завершена
                // Можно добавить логику завершения гонки, например, вывод результатов
            }
            else
            {
                // Начало нового круга
                ResetLap();
            }
        }
    }

    private void ResetLap()
    {
        lapTime = 0f;
        pointsPassed = 0;
        routePoints.Clear();

        // Перезапускаем точки маршрута
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        foreach (GameObject point in points)
        {
            routePoints.Add(point.GetComponent<Collider>());
        }

        finishLine.enabled = false; // Отключаем финиш до следующего прохождения всех точек
    }

    private void UpdateUI()
    {
        lapText.text = $"Круг: {currentLap} / {totalLaps}";
        timeText.text = $"Время: {lapTime:F2} сек";
        pointText.text = $"Точки: {pointsPassed} / {totalPoints}";
    }
}
