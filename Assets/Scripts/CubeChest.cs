using UnityEngine;

public class CubeChest : MonoBehaviour
{
    //[SerializeField] AudioClip audioClipUseBonus;
    private AudioSource audioSource;
    private bool isCollision = true;
    private GameController gameController;
    private ReadLine readLine;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        audioSource = GetComponent<AudioSource>();
        readLine = FindObjectOfType<ReadLine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube") && isCollision)
        {
            //audioSource.clip = audioClipUseBonus;
            //audioSource.Play();
            readLine.SoundForBonusCube();

            isCollision = false;

            Vector3 posCube = other.transform.position;
            int i = other.GetComponent<Cube>().numberCube;
            gameController.listCube.Remove(other.GetComponent<Cube>());
            Destroy(other.gameObject);
            Instantiate(gameController.cubesMain[i + 1], posCube, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
