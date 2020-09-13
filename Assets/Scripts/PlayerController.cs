using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public PlayerConfig playerConfig;

    public PhotonView pv;

    public CharacterController cc;

    float walkSpeed;
    float rotateSpeed;
    
    public Transform cam;

    float hor;
    float ver;
    
    public Transform myEmote;

    public Vector3 remotePosition;
    public Vector3 remoteRotation;

    public Coroutine emoteDisappearRoutine;

    public bool paused;


    private Vector3 offsetX;
    private Vector3 offsetY;

    float mouseX;
    float mouseY;

    public Transform cameraTarget;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        offsetX = new Vector3(0, playerConfig.cameraHeight, playerConfig.cameraDistance);
        offsetY = new Vector3(0, 0, playerConfig.cameraDistance);
    }

    void Start()
    {
        if (pv.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            walkSpeed = playerConfig.walkSpeed;
            rotateSpeed = playerConfig.rotateSpeed;

            cc = GetComponent<CharacterController>();

            cam = Camera.main.transform;
            cameraTarget = new GameObject("CameraAnchor").transform;
            cameraTarget.parent = transform;
            cam.parent = cameraTarget;
            
            UIController.uIController.myPlayer = this;

        }
        else
        {
            Destroy(gameObject.GetComponent<CharacterController>());
        }


        myEmote = transform.GetChild(0);
    }
    
    void Update()
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIController.uIController.ToggleEscapeMenu();
            }

            if (!paused)
            {
                ver = Input.GetAxisRaw("Vertical");
                hor = Input.GetAxisRaw("Horizontal");

                var forwardVector = transform.forward.normalized * walkSpeed * Time.deltaTime * ver;
                var horizontalVector = transform.right.normalized * walkSpeed * Time.deltaTime * hor;

                cc.Move(forwardVector + horizontalVector);

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    UIController.uIController.ToggleEmoteMenu();
                }

                transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraTarget.eulerAngles.y, transform.eulerAngles.z);
            }
        }
    }

    void LateUpdate()
    {
        if (pv.IsMine)
        {
            if(!paused)
            {
                TiltCamera();
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, remotePosition, 0.1f);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, remoteRotation, 0.1f);
            myEmote.transform.eulerAngles = new Vector3(myEmote.transform.eulerAngles.x, PhotonLobby.photonLobby.localPlayer.transform.eulerAngles.y, myEmote.transform.eulerAngles.z);
        }
    }

    void TiltCamera()
    {
        mouseX += Input.GetAxis("Mouse X") * playerConfig.cameraTiltSpeed * Time.deltaTime;
        mouseY += -Input.GetAxis("Mouse Y") * playerConfig.cameraTiltSpeed * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, playerConfig.cameraTiltRange.x, playerConfig.cameraTiltRange.y);

        cameraTarget.localPosition = new Vector3(0, playerConfig.cameraHeight, 0);
        cam.localPosition = new Vector3(0, 0, -playerConfig.cameraDistance);

        cam.LookAt(cameraTarget);

        cameraTarget.rotation = Quaternion.Euler(mouseY, mouseX, 0);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.eulerAngles);
        }
        else
        {
            remotePosition = (Vector3)stream.ReceiveNext();
            remoteRotation = (Vector3)stream.ReceiveNext();
        }
    }
}
