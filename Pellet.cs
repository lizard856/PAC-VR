using UnityEngine;

public class Pellet : MonoBehaviour
{
    public bool eaten { get; private set; }

    public void Eat()
    {
        if (eaten) return;
        eaten = true;
        gameObject.SetActive(false);

        // tell the manager
        PacGameManager.Instance.PelletEaten(this);
    }

    public void ResetPellet()
    {
        eaten = false;
        gameObject.SetActive(true);
    }
}
