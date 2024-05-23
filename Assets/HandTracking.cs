using UnityEngine;

public class HandTracking : MonoBehaviour
{
    public UDPReceive udpReceive;
    public GameObject[] handPointsLeft;
    public GameObject[] handPointsRight;

    void Update()
    {
        // Get the data from the UDPReceive script and parse it
        string data = udpReceive.data;
        data = data.Remove(0, 1);
        data = data.Remove(data.Length - 1, 1);
        string[] points = data.Split(',');

        bool noLeftHand = true;
        for (int i = 0; i < 21; i++)
        {
            if (noLeftHand)
            {
                if (points[i * 3] != "0" || points[i * 3 + 1] != "0" || points[i * 3 + 2] != "0")
                {
                    noLeftHand = false;
                }
            }

            float x = 7 - float.Parse(points[i * 3]) / 70;
            float y = float.Parse(points[i * 3 + 1]) / 100;
            float z = -float.Parse(points[i * 3 + 2]) / 100;

            handPointsLeft[i].transform.localPosition = new Vector3(x, y, z);
        }
        if (noLeftHand)
        {
            for (int i = 0; i < 21; i++)
            {
                handPointsLeft[i].transform.localPosition = new Vector3(0, -50, 0);
            }
        }

        bool noRightHand = true;
        for (int i = 21; i < 42; i++)
        {
            if (noRightHand)
            {
                if (points[i * 3] != "0" || points[i * 3 + 1] != "0" || points[i * 3 + 2] != "0")
                {
                    noRightHand = false;
                }
            }
            float x = 7 - float.Parse(points[i * 3]) / 70;
            float y = float.Parse(points[i * 3 + 1]) / 100;
            float z = -float.Parse(points[i * 3 + 2]) / 100;

            handPointsRight[i - 21].transform.localPosition = new Vector3(x, y, z);
        }
        if (noRightHand)
        {
            for (int i = 0; i < 21; i++)
            {
                handPointsRight[i].transform.localPosition = new Vector3(0, -50, 0);
            }
        }
    }
}
