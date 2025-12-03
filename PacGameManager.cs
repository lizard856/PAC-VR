using UnityEngine;

public class PacGameManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI pelletText;
    public static PacGameManager Instance { get; private set; }

    [Header("XR")]
    public RecenterOrigin recenterOrigin;


    [Header("Player")]
    public Transform playerRoot;   // PacMover
    public Transform playerHead;   // XR camera (optional)

    [Header("Ghosts")]
    [SerializeField] private Transform[] ghosts;
    public float ghostCatchRadius = 0.5f;

    [Header("Pellets")]
    public float eatRadius = 0.4f;
    public AudioSource audioSource;
    public AudioClip eatSound;
    public AudioClip deadSound;
    public AudioClip completeSound;

    Vector3 startRootPos;
    Vector3[] ghostStartPos;

    Pellet[] pellets;
    int pelletsRemaining;

    Vector3 PlayerLogicPos
    {
        get
        {
            // gameplay position is always the PacMover root
            return playerRoot.position;
        }
    }

    void Awake()
    {
        Instance = this;

        // store player start
        startRootPos = playerRoot.position;

        // store all ghost start positions
        if (ghosts != null && ghosts.Length > 0)
        {
            ghostStartPos = new Vector3[ghosts.Length];
            for (int i = 0; i < ghosts.Length; i++)
            {
                if (ghosts[i] != null)
                    ghostStartPos[i] = ghosts[i].position;
            }
        }
    }

    void Start()
    {
        pellets = FindObjectsOfType<Pellet>(true);
        pelletsRemaining = pellets.Length;
        UpdatePelletUI();
    }

    void Update()
    {
        CheckPelletsAroundPlayer();
        CheckGhostCollision();
    }

    void CheckPelletsAroundPlayer()
    {
        if (pellets == null || pellets.Length == 0) return;

        float r2 = eatRadius * eatRadius;
        Vector3 p = PlayerLogicPos;

        foreach (var pel in pellets)
        {
            if (!pel.gameObject.activeSelf) continue;

            Vector3 d = pel.transform.position - p;
            if (d.sqrMagnitude <= r2)
                pel.Eat();
        }
    }

    void CheckGhostCollision()
    {
        if (ghosts == null || ghosts.Length == 0) return;

        float r2 = ghostCatchRadius * ghostCatchRadius;
        Vector3 p = PlayerLogicPos;

        foreach (var g in ghosts)
        {
            if (g == null) continue;

            Vector3 d = g.position - p;
            if (d.sqrMagnitude <= r2)
            {
                ResetLevel(true);
                return;   // one ghost is enough
            }
        }
    }

    public void PelletEaten(Pellet pel)
    {
        pelletsRemaining--;

        if (audioSource != null && eatSound != null)
            audioSource.PlayOneShot(eatSound);

        UpdatePelletUI();

        // LEVEL COMPLETE
        if (pelletsRemaining <= 0)
        {
            if (audioSource != null && completeSound != null)
                audioSource.PlayOneShot(completeSound);

            ResetLevel(false);   // false = not ghost-caused
            return;
        }
    }


   public void ResetLevel(bool ghostCaught)
    {
        if (ghostCaught && audioSource != null && deadSound != null)
            audioSource.PlayOneShot(deadSound);

        // reset pellets
        if (pellets != null)
        {
            foreach (var p in pellets)
                p.ResetPellet();

            pelletsRemaining = pellets.Length;
            UpdatePelletUI();
        }

        // reset player root to start tile
        playerRoot.position = startRootPos;

        // recenter XR Origin based on current HMD pose
        if (recenterOrigin != null)
            recenterOrigin.Recenter();

        // reset all ghosts
        if (ghosts != null)
        {
            for (int i = 0; i < ghosts.Length; i++)
            {
                var g = ghosts[i];
                if (g == null) continue;

                var bfs = g.GetComponent<GhostBFS>();
                if (bfs != null)
                    bfs.ResetGhost();

                var basic = g.GetComponent<PathFindingBasic>();
                if (basic != null)
                    basic.ResetPath();
            }
        }
    }


    void UpdatePelletUI()
    {
        if (pelletText != null)
            pelletText.text = "Pellets: " + pelletsRemaining;
    }
}
