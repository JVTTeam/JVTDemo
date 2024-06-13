using UnityEngine;

public class HandTracking : MonoBehaviour
{
    public UDPReceive udpReceive;
    public GameObject[] handPointsLeft;
    public GameObject[] handPointsRight;

    readonly float[] boneLengths = {
        0.8394833218698304f,        // 0 - 1
        0.0013058324934224067f,     // 1 - 2
        0.001556713449901446f,      // 2 - 3
        0.8470333386586592f,        // 3 - 4
        0.942241988192333f,         // 0 - 5
        0.0021939160932196045f,     // 5 - 6
        0.0006260465040053635f,     // 6 - 7
        0.896074264976329f,         // 7 - 8
        0.9476952046946703f,        // 0 - 9
        0.0022327868482407216f,     // 9 - 10
        0.0012573068188078123f,     // 10 - 11
        0.8970807565036689f,        // 11 - 12
        0.9440999469617241f,        // 0 - 13
        0.001892961544691598f,      // 13 - 14
        0.001008532419285475f,      // 14 - 15
        0.8958698355058643f,        // 15 - 16
        0.9434872804207006f,        // 0 - 17
        0.0006148126847044153f,     // 17 - 18
        0.0006719074590229164f,     // 18 - 19
        0.8905788226406183f         // 19 - 20
    };

    (float, float, float) PointCalc((float x, float y, float z) p1, (float x, float y, float z) p2, float actualDistance)
    {
        float measuredDistance = Mathf.Sqrt(Mathf.Pow(p1.x - p2.x, 2) + Mathf.Pow(p1.y - p2.y, 2) + Mathf.Pow(p1.z - p2.z, 2));
        float ratio = actualDistance / measuredDistance;
        return ((p2.x - p1.x) * ratio + p1.x, (p2.y - p1.y) * ratio + p1.y, (p2.z - p1.z) * ratio + p1.z);
    }

    (float, float, float) GetPointCoords(int p, string[] points)
    {
        return (float.Parse(points[p * 3]) / 800, float.Parse(points[p * 3 + 1]) / 600 + 1, float.Parse(points[p * 3 + 2]));
    }

    void Update()
    {
        // Get the data from the UDPReceive script and parse it
        string data = udpReceive.data;
        data = data.Remove(0, 1);
        data = data.Remove(data.Length - 1, 1);
        string[] points = data.Split(',');

        // for (int i = 0; i < 21; i++)
        // {
        //     // float x = 7f - float.Parse(points[i * 3]) / 1280 - 5.8f;
        //     // float y = float.Parse(points[i * 3 + 1]) / 720 + 0.8f;
        //     // float z = -float.Parse(points[i * 3 + 2]) / 720;

        //     handPointsLeft[i].transform.localPosition = new Vector3(x, y, z);
        // }

        float leftSize = Mathf.Sqrt(Mathf.Pow(GetPointCoords(0, points).Item1 - GetPointCoords(12, points).Item1, 2) + Mathf.Pow(GetPointCoords(0, points).Item2 - GetPointCoords(12, points).Item2, 2) + Mathf.Pow(GetPointCoords(0, points).Item3 - GetPointCoords(12, points).Item3, 2));
        float leftDistFromScreen = Mathf.Sqrt(leftSize) - 32;

        (float, float, float)[] leftPointCoords = new (float, float, float)[21];

        leftPointCoords[0] = (0.719307005f, 1.44475961f, 0.137918949f);
        //leftPointCoords[0] = (GetPointCoords(0, points).Item1 / 2, GetPointCoords(0, points).Item2 / 2, (-leftDistFromScreen / 8f - 3.2f) / 2);

        for (int i = 1; i < 21; i++)
        {
            int rootPoint = i - 1;
            if (i == 5 || i == 9 || i == 13 || i == 17)
                rootPoint = 0;
            leftPointCoords[i] = PointCalc(leftPointCoords[rootPoint], GetPointCoords(i, points), boneLengths[i - 1]);
        }
        for (int i = 0; i < 21; i++)
        {
            handPointsLeft[i].transform.localPosition = new Vector3(leftPointCoords[i].Item1, leftPointCoords[i].Item2, leftPointCoords[i].Item3);
        }
    }
}
