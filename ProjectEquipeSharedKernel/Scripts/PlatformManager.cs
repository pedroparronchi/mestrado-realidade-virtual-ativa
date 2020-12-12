using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using UnityStandardAssets.Characters.FirstPerson;
using VRStandardAssets.Utils;

public enum VRPlataform
{
    PC = 0, 
    Oculus = 1,
    OculusQuest = 2
}


public enum VRControlScheme
{
    Laser = 0,
    Grab = 1
}

public class PlatformManager : MonoBehaviour
{
    private VRPlataform currentVRPlatform = VRPlataform.PC;
    string loadedDeviceName;

    [SerializeField] GameObject pcCam, oculusCam, hudRPC, hudRRV, hudLPC, hudLRV;
    MonoBehaviour pcController, oculusController;
        
    Transform rightHand, leftHand;

    private VRControlScheme currentVRControlScheme = VRControlScheme.Laser;
    VRRaycaster vrRaycaster;
    public GameObject vrMode;
    float countDown;

    public bool TestarRVNoPC;

    public VRControlScheme CurrentVRControlScheme { get => currentVRControlScheme;}
    public VRPlataform CurrentVRPlatform { get => currentVRPlatform;}

    // Start is called before the first frame update
    public void Awake()
    {
        currentVRPlatform = VRPlataform.PC;

        if(TestarRVNoPC)
            currentVRPlatform = VRPlataform.OculusQuest;

        pcController = (FirstPersonController)FindObjectOfType(typeof(FirstPersonController));
        oculusController = (OVRPlayerController)FindObjectOfType(typeof(OVRPlayerController));

        if (XRDevice.isPresent)
        {         
            OnDeviceLoadAction(XRDevice.model);
        }
       // else
       // {
       //     XRDevice.deviceLoaded += OnDeviceLoadAction;
       // }
        SetPlatform();
        if (CurrentVRPlatform == VRPlataform.PC)
            Cursor.visible = false;
    }

    private void Start()
    {
        vrRaycaster = ReferenceManagerIndependent.Instance.VRRaycaster;
    }

    void OnDeviceLoadAction(string newLoadedDeviceName)
    {
        loadedDeviceName = newLoadedDeviceName;        
        if (loadedDeviceName.ToLower().Contains("oculus"))
        {
            currentVRPlatform = VRPlataform.OculusQuest;
        }        
    }

    private void SetPlatform()
    {
        bool isPc = false, isOculus = false;
        switch (CurrentVRPlatform)
        {
            case VRPlataform.PC:
            default:
                isPc = true;
                break;
            case VRPlataform.Oculus:
            case VRPlataform.OculusQuest:
                isOculus = true;
                break;
        }
        pcCam?.SetActive(isPc);
        oculusCam?.SetActive(isOculus);

#if NESTLE_RV
        if (hudRPC != hudRRV)
        {
            if (!isPc && hudRPC)
            {
                hudRPC.SetActive(false);
                DestroyImmediate(hudRPC);
            }
            if (!isOculus && hudRRV)
            {
                hudRRV.SetActive(false);
                DestroyImmediate(hudRRV);
            }
        }
        if (hudLPC != hudLRV)
        {
            if (!isPc && hudLPC)
            {
                hudLPC.SetActive(false);
                DestroyImmediate(hudLPC);
            }
            if (!isOculus && hudLRV)
            {
                hudLRV.SetActive(false);
                DestroyImmediate(hudLRV);
            }
        }

#endif

        pcController.enabled = isPc;
        oculusController.enabled = isOculus;

        if (isOculus)
            SetHandsTransforms();

        ReferenceManagerIndependent.Instance.UpdateVRObjects();

    }
    
    void SetHandsTransforms()
    {
        rightHand = GameObject.Find("RightHandAnchor").transform;
        leftHand = GameObject.Find("LeftHandAnchor").transform;
    }

    public Transform GetRightHand()
    {
        if (!rightHand)
            Debug.LogError("Acessing null right hand");
        return rightHand;
    }

    public Transform GetLeftHand()
    {
        if (!leftHand)
            Debug.LogError("Acessing null left hand");
        return leftHand;
    }

    public MonoBehaviour GetPlayerController()
    {
        if(CurrentVRPlatform == VRPlataform.PC)
        {
            return pcController;
        }
        else
        {
            return oculusController;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("ControlScheme") || Input.GetButtonDown("Oculus_CrossPlatform_Button1"))
        {
            ChangeVRControlScheme();            
        }
        if(countDown > 0)
        {
            countDown -= Time.deltaTime;
            if (countDown < 0)
            {
                vrMode.SetActive(false);
            }
        }
       
    }

    public void ChangeVRControlScheme()
    {
        currentVRControlScheme = CurrentVRControlScheme == VRControlScheme.Laser ? VRControlScheme.Grab : VRControlScheme.Laser;
        vrRaycaster.ResetInteractable();
        if (CurrentVRControlScheme == VRControlScheme.Laser)
        {
            vrRaycaster.GetReticle(true).Show();
            vrRaycaster.SetLine(show: true, isRight: true);
            vrRaycaster.rightFingerIndicator.gameObject.SetActive(false);
        }
        else
        {
            vrRaycaster.GetReticle(true).Hide();
            vrRaycaster.SetLine(show: false, isRight: true);
            vrRaycaster.rightFingerIndicator.gameObject.SetActive(true);
        }
        if (vrMode)
        {
            vrMode.SetActive(true);
            vrMode.GetComponent<TextMeshProUGUI>().text = ("Você trocou o modo de controle para " + CurrentVRControlScheme);
            countDown = 3;
        }
    }

    public void SetVRControlScheme(VRControlScheme newVRControlScheme)
    {
        currentVRControlScheme = newVRControlScheme == VRControlScheme.Laser ? VRControlScheme.Grab : VRControlScheme.Laser;
        ChangeVRControlScheme();
    }

}
