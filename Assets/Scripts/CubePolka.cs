using UnityEngine;

public class CubePolka : MonoBehaviour
{
    private bool isCollision = true;
    private GameController gameController;
    public float count;
    private int minNumberCube = 100;
    private int numberNewCube;
    private Cube currentCube;
    private ReadLine readLine;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        readLine = FindObjectOfType<ReadLine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube") && isCollision)
        {
            readLine.SoundForBonusCube();

            isCollision = false;
            count = 0;

            for (int i = 0; i < gameController.listCube.Count; i++)
            {
                minNumberCube = Mathf.Min(minNumberCube, gameController.listCube[i].numberCube);
            }

            for (int i = gameController.listCube.Count - 1; i >= 0; i-- /*int i = 0; i < gameController.listCube.Count; i++*/)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (gameController.listCube[i].numberCube == minNumberCube+j)
                    {
                        count += Mathf.Pow(2, (minNumberCube + j+1));

                        currentCube = gameController.listCube[i];
                        gameController.listCube.Remove(gameController.listCube[i].GetComponent<Cube>());
                        Destroy(currentCube.gameObject);
                        break;
                    }

                }
            }

            for (int i = 0; i < 25; i++)
            {
                if(count == Mathf.Pow(2,i))
                {
                    numberNewCube = i-1;
                    break;
                }
                else if(count < Mathf.Pow(2, i))
                {
                    numberNewCube = i-1;
                    break;
                }
            }
            Instantiate(gameController.cubesMain[numberNewCube], transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
