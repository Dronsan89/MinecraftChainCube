using UnityEngine;

public class Cube : MonoBehaviour
{
    public int numberCube;
    public int IDCube;
    public bool isActiveCube = true;

    private bool isInst = true;
    private GameController gameController;
    private ReadLine readLine;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        gameController.listCube.Add(this);
        readLine = FindObjectOfType<ReadLine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube") && other.GetComponent<Cube>().numberCube == numberCube && isActiveCube)
        {
            gameController.listCube.Remove(other.GetComponent<Cube>());
            Destroy(other.gameObject);
            if (other.GetComponent<Cube>().IDCube < IDCube && isInst)
            {
                isInst = false;
                gameController.CollisionCube(this);
            }
        }
        else if (other.CompareTag("Cube") || other.CompareTag("other"))
        {
            if (transform.childCount == 7)
                Destroy(transform.GetChild(6).gameObject);
            readLine.SoundForCube();
        }
    }
}
