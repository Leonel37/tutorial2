using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{

   [SerializeField] private TextMesh healthText;
    [SerializeField] private Renderer rendereMaterial;

    [SyncVar(hook = nameof(WhenHealthChange))]
    [SerializeField] private int Health;

    [SyncVar(hook = nameof(WhenColorChange))]
    [SerializeField] private Color color;

    [SerializeField] private Rigidbody rb; 
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnT;
    [SerializeField] private Light light;

    #region Servidor
    [Server]

    public void SetHealthPlayer(int Health)
    {
        this.Health = Health;    
        
    }

    [Server]
    public void SetColorPlayer(Color color)
    {
        this.color = color;
    }


    [Command]
    public void CmdSetHealthPlayer(int health)
    {
        SetHealthPlayer(health);
    }
    #endregion 

    #region Cliente
    public void WhenHealthChange(int OldHealth, int NewHealth)
    {
        healthText.text = NewHealth.ToString();

    }

    public void WhenColorChange(Color OldColor, Color NewColor)
    {
       rendereMaterial.material.SetColor("_BaseColor", NewColor);

    }


    [ContextMenu("Cambiar la vida de cliente")]
    public void SetMyHealth(){
        CmdSetHealthPlayer(8);
    }

    private void Start()
    {
        if(isLocalPlayer)
        {
            light.enabled = true;
        }else{
            light.enabled = false;
        }
    }
    

    private void Update(){
        if(isLocalPlayer)
        {
            if(Input.GetMouseButtonDown(0))
            {
                CmdFire();
            }
        }
    }


    private void FixedUpdate()
    {
        if(isLocalPlayer)
        {
            Vector3 inputAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            rb.velocity = inputAxis * 500 * Time.fixedDeltaTime;

            Rotate();
        }
    }
    [Command]
        void CmdFire()
        {
            GameObject projectile = Instantiate(projectilePrefab, spawnT.position, spawnT.rotation);
            NetworkServer.Spawn(projectile);
           
        }

     void Rotate()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                Debug.DrawLine(ray.origin, hit.point);
                Vector3 lookRotation = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                transform.LookAt(lookRotation);
            }
        }


[ServerCallback]
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                --Health;
                if (Health == 0)
                    NetworkServer.Destroy(gameObject);
            }
        }

    #endregion
}

