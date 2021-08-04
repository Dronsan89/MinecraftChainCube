using System.Collections;
using UnityEngine;

public class CubeZhele : MonoBehaviour
{
    [SerializeField] GameObject cubeZheleMini;

    private bool isCollision = true;
    private GameController gameController;
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

            for (int i = 0; i < gameController.listCube.Count; i++)
            {
                int randY = Random.Range(3, 5);
                gameController.listCube[i].GetComponent<Rigidbody>().useGravity = false;
                gameController.listCube[i].transform.position += Vector3.up * randY;
                gameController.listCube[i].isActiveCube = false;
                int randX = Random.Range(-3, 3);
                int randZ = Random.Range(-3, 1);
                gameController.listCube[i].GetComponent<Rigidbody>().AddForce(new Vector3(randX, 0, randZ) * 200);
            }
            StartCoroutine(PauseCube());
        }

        if (other.CompareTag("Finish"))
        {
            Instantiate(cubeZheleMini, new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z), Quaternion.identity);
            Instantiate(cubeZheleMini, new Vector3(transform.position.x + 0.7f, transform.position.y, transform.position.z), Quaternion.identity);
            Instantiate(cubeZheleMini, new Vector3(transform.position.x - 0.7f, transform.position.y, transform.position.z), Quaternion.identity);
            Instantiate(cubeZheleMini, new Vector3(transform.position.x - 2f, transform.position.y, transform.position.z), Quaternion.identity);
            transform.position = new Vector3(1000,1000, 1000);
        }
    }

    IEnumerator PauseCube()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Rigidbody>().AddForce(-Vector3.forward * 800);
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < gameController.listCube.Count; i++)
        {
            gameController.listCube[i].GetComponent<Rigidbody>().useGravity = true;
            gameController.listCube[i].isActiveCube = true;
        }
        Destroy(gameObject);
    }
}
