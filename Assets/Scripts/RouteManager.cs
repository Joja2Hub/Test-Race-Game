using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouteManager : MonoBehaviour
{
    public TextMeshProUGUI lapText; // UI ������� ��� ����������� �������� �����
    public TextMeshProUGUI timeText; // UI ������� ��� ����������� �������
    public TextMeshProUGUI pointText; // UI ������� ��� ����������� ���������� ���������� �����

    public int totalLaps = 2; // ����� ���������� ������
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
        // ����� ��� ����� ��������
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        totalPoints = points.Length;
        foreach (GameObject point in points)
        {
            routePoints.Add(point.GetComponent<Collider>());
        }

        // ����� �������� ������ � ������
        startLine = GameObject.FindGameObjectWithTag("StartLine").GetComponent<Collider>();
        finishLine = GameObject.FindGameObjectWithTag("FinishLine").GetComponent<Collider>();
        finishLine.enabled = false; // ��������� ������� ������ �� ����������� ���� �����

        UpdateUI();
    }

    void Update()
    {
        if (isRacing)
        {
            lapTime += Time.deltaTime; // ������� ����� �����
            UpdateUI();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StartLine") && !isRacing)
        {
            // ������ �����
            isRacing = true;
            startLine.enabled = false; // ��������� ������� ������ ����� �����������
        }
        else if (other.CompareTag("Point"))
        {
            if (routePoints.Contains(other))
            {
                routePoints.Remove(other); // ������� ����� �� ������ ������������
                pointsPassed++;
                if (routePoints.Count == 0)
                {
                    finishLine.enabled = true; // �������� �����, ���� ��� ����� ��������
                }
            }
        }
        else if (other.CompareTag("FinishLine") && isRacing && routePoints.Count == 0)
        {
            // ����� �����
            currentLap++;
            if (currentLap > totalLaps)
            {
                isRacing = false; // ����� ���������
                // ����� �������� ������ ���������� �����, ��������, ����� �����������
            }
            else
            {
                // ������ ������ �����
                ResetLap();
            }
        }
    }

    private void ResetLap()
    {
        lapTime = 0f;
        pointsPassed = 0;
        routePoints.Clear();

        // ������������� ����� ��������
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        foreach (GameObject point in points)
        {
            routePoints.Add(point.GetComponent<Collider>());
        }

        finishLine.enabled = false; // ��������� ����� �� ���������� ����������� ���� �����
    }

    private void UpdateUI()
    {
        lapText.text = $"����: {currentLap} / {totalLaps}";
        timeText.text = $"�����: {lapTime:F2} ���";
        pointText.text = $"�����: {pointsPassed} / {totalPoints}";
    }
}
