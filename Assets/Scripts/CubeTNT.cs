using System.Collections.Generic;
using UnityEngine;

public class CubeTNT : MonoBehaviour
{
    //[SerializeField] AudioClip audioClipUseBonus;
    private AudioSource audioSource;
    [SerializeField] GameObject deadCube;
    [SerializeField] int radiusActive;

    private GameController gameController;
    private bool isCollision = true;
    private Cube currentCube;
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
            for (int i = gameController.listCube.Count - 1; i >= 0; i--/*int i = 0; i < gameController.listCube.Count; i++*/)
            {
                if (gameController.listCube[i] != null)
                {
                    Vector3 posCube = gameController.listCube[i].transform.position;
                    float dist = Vector3.Distance(posCube, transform.position);

                    if (dist <= radiusActive)
                    {
                        currentCube = gameController.listCube[i];
                        gameController.listCube.Remove(gameController.listCube[i].GetComponent<Cube>());
                        Destroy(currentCube.gameObject);
                        Instantiate(deadCube, new Vector3(posCube.x + 0.5f, posCube.y, posCube.z + 0.5f), Quaternion.identity);
                        Instantiate(deadCube, new Vector3(posCube.x + 0.5f, posCube.y, posCube.z - 0.5f), Quaternion.identity);
                        Instantiate(deadCube, new Vector3(posCube.x - 0.5f, posCube.y, posCube.z + 0.5f), Quaternion.identity);
                        Instantiate(deadCube, new Vector3(posCube.x - 0.5f, posCube.y, posCube.z - 0.5f), Quaternion.identity);
                    }
                    if (gameController.listCube == null) break;
                }
            }
            Destroy(gameObject);
        }
    }
}
