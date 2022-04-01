using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace EllipseDrawer
{
    internal class EllipseDrawer : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField abLengthText;

        [SerializeField]
        private TMP_InputField ap2AbRatioText;

        [SerializeField]
        private TMP_InputField speedText;

        [SerializeField]
        private TMP_InputField pointsCountText;

        [SerializeField]
        private GameObject pointA;

        [SerializeField]
        private GameObject aLabel;

        [SerializeField]
        private GameObject pointB;

        [SerializeField]
        private GameObject bLabel;

        [SerializeField]
        private GameObject pointP;

        [SerializeField]
        private GameObject pLabel;

        [SerializeField]
        private LineRenderer abLineRenderer;

        [SerializeField]
        private LineRenderer ellipseLineRenderer;

        private Vector3 labelOffest = new(0.2f, -0.2f, 0);

        private IEnumerator currentPlottingCoroutine = null;

        private IEnumerator StartPlottingCoroutine()
        {
            abLineRenderer.positionCount = 0;
            abLineRenderer.SetPositions(new Vector3[0]);
            abLineRenderer.startWidth = 0.05f;
            abLineRenderer.endWidth = 0.05f;
            ellipseLineRenderer.positionCount = 0;
            ellipseLineRenderer.SetPositions(new Vector3[0]);
            ellipseLineRenderer.startWidth = 0.05f;
            ellipseLineRenderer.endWidth = 0.05f;
            pointA.SetActive(false);
            aLabel.SetActive(false);
            pointB.SetActive(false);
            bLabel.SetActive(false);
            pointP.SetActive(false);
            pLabel.SetActive(false);
            if (!float.TryParse(abLengthText.text, out float abLength) || abLength <= 0)
            {
                yield break;
            }
            if (!float.TryParse(ap2AbRatioText.text, out float ab2AbRatio) || ab2AbRatio <= 0)
            {
                yield break;
            }
            if (!int.TryParse(speedText.text, out int speed) || speed <= 0)
            {
                yield break;
            }
            if (!int.TryParse(pointsCountText.text, out int pointsCount) || pointsCount <= 0)
            {
                yield break;
            }
            int pointsCalculatedThisFrame = 0;
            abLineRenderer.positionCount = 2;
            ellipseLineRenderer.positionCount = pointsCount + 1;
            pointA.SetActive(true);
            aLabel.SetActive(true);
            pointB.SetActive(true);
            bLabel.SetActive(true);
            pointP.SetActive(true);
            pLabel.SetActive(true);
            int totalPointsCalculated = 0;
            while (true)
            {
                for (int i = 0; i < pointsCount / 2; i++, pointsCalculatedThisFrame++, totalPointsCalculated++)
                {
                    if (pointsCalculatedThisFrame == speed)
                    {
                        pointsCalculatedThisFrame = 0;
                        yield return null;
                    }
                    abLineRenderer.SetPositions(new Vector3[0]);
                    Vector3 a = new(abLength - (float)i / pointsCount * abLength * 4, 0, 0);
                    Vector3 b = new(0, Mathf.Sqrt(abLength * abLength - a.x * a.x), 0);
                    Vector3 p = Vector3.LerpUnclamped(a, b, ab2AbRatio);
                    pointA.transform.position = a;
                    aLabel.transform.position = a + labelOffest;
                    pointB.transform.position = b;
                    bLabel.transform.position = b + labelOffest;
                    pointP.transform.position = p;
                    pLabel.transform.position = p + labelOffest;
                    abLineRenderer.SetPositions(new Vector3[] { a, b });
                    if (totalPointsCalculated <= pointsCount)
                    {
                        ellipseLineRenderer.SetPosition(i, p);
                    }
                }
                for (int i = 0; i < pointsCount - pointsCount / 2 + 1; i++, pointsCalculatedThisFrame++, totalPointsCalculated++)
                {
                    if (pointsCalculatedThisFrame == speed)
                    {
                        pointsCalculatedThisFrame = 0;
                        yield return null;
                    }
                    abLineRenderer.SetPositions(new Vector3[0]);
                    Vector3 a = new(-abLength + (float)i / pointsCount * abLength * 4, 0, 0);
                    Vector3 b = new(0, -Mathf.Sqrt(abLength * abLength - a.x * a.x), 0);
                    Vector3 p = Vector3.LerpUnclamped(a, b, ab2AbRatio);
                    pointA.transform.position = a;
                    aLabel.transform.position = a + labelOffest;
                    pointB.transform.position = b;
                    bLabel.transform.position = b + labelOffest;
                    pointP.transform.position = p;
                    pLabel.transform.position = p + labelOffest;
                    abLineRenderer.SetPositions(new Vector3[] { a, b });
                    if (totalPointsCalculated <= pointsCount)
                    {
                        ellipseLineRenderer.SetPosition(i + pointsCount / 2, p);
                    }
                }
            }
        }

        public void StartPlotting()
        {
            if (currentPlottingCoroutine is not null)
            {
                StopCoroutine(currentPlottingCoroutine);
            }
            currentPlottingCoroutine = StartPlottingCoroutine();
            StartCoroutine(currentPlottingCoroutine);
        }
    }
}
