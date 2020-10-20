using System.Collections;
using UnityEngine;

public class CreatePrefabAfterSecondsBehaviour : MonoBehaviour
{
    public float _secondsToSpawn = 1;
    public GameObject _Square = null;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExampleCoroutine());
    }

    private IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(_secondsToSpawn);
        Instantiate(_Square);
    }

}
