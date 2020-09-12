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

    void Awake()
    {
        pv = GetComponent<PhotonView>();
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
            if (!paused)
            {
                ver = Input.GetAxisRaw("Vertical");
                hor = Input.GetAxisRaw("Horizontal");

                transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime, 0);

                var forwardVector = transform.forward.normalized * walkSpeed * Time.deltaTime * ver;
                var horizontalVector = transform.right.normalized * walkSpeed * Time.deltaTime * hor;

                cc.Move(forwardVector + horizontalVector);

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    UIController.uIController.ToggleEmoteMenu();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIController.uIController.ToggleEscapeMenu();
            }


            var camPos = (transform.forward.normalized * -playerConfig.cameraDistance) + transform.position;
            camPos.y = playerConfig.cameraHeight;
            cam.position = camPos;

            float tiltFactor = playerConfig.cameraTiltSpeed * -Input.GetAxis("Mouse Y") * Time.deltaTime;
            float clampedXRotation = Mathf.Clamp(cam.transform.eulerAngles.x + tiltFactor, playerConfig.cameraTiltRange.x, playerConfig.cameraTiltRange.y);
            cam.eulerAngles = new Vector3(clampedXRotation, transform.eulerAngles.y, 0);
        }
    }

    void LateUpdate()
    {
        if (!pv.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, remotePosition, 0.1f);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, remoteRotation, 0.1f);
            myEmote.transform.eulerAngles = new Vector3(myEmote.transform.eulerAngles.x, PhotonLobby.photonLobby.localPlayer.transform.eulerAngles.y, myEmote.transform.eulerAngles.z);
        }
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
