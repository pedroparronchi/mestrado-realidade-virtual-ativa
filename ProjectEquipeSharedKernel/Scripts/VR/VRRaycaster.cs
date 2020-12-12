using System;
using TMPro;
using UnityEngine;

namespace VRStandardAssets.Utils
{
    // In order to interact with objects in the scene
    // this class casts a ray into the scene and if it finds
    // a VRInteractiveItem it exposes it for other classes to use.
    // This script should be generally be placed on the camera.
    public class VRRaycaster : MonoBehaviour
    {
        public event Action<RaycastHit> OnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.
        public event Action<VRInteractiveItem> OnOverSomething, OnOutOfEverything;


        [SerializeField] private Transform m_Camera;
        [SerializeField] private LayerMask m_UILayers;                  // Layers to prioritize
        [SerializeField] private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
        [SerializeField] private Reticle m_ReticleL;                     // The reticle, if applicable.
        [SerializeField] private Reticle m_ReticleR;                     // The reticle, if applicable.
        [SerializeField] private VRInput m_VrInput;                     // Used to call input based events on the current VRInteractiveItem.
        [SerializeField] private bool m_ShowDebugRay;                   // Optionally show the debug ray.
        [SerializeField] private float m_DebugRayLength = 5f;           // Debug ray length.
        [SerializeField] private float m_DebugRayDuration = 1f;         // How long the Debug ray will remain visible.
        [SerializeField] private float m_RayLength = 10f;              // How far into the scene the ray is cast.
        [SerializeField] private float m_RayRadius = 0.05f;              // Radius of the spherecast.

        [SerializeField] private LineRenderer m_LineRendererL = null; // For supporting Laser Pointer
        [SerializeField] private LineRenderer m_LineRendererR = null; // For supporting Laser Pointer
        public bool ShowLineRenderer = true;                         // Laser pointer visibility
        [SerializeField] private Transform m_TrackingSpace = null;   // Tracking space (for line renderer)

        [SerializeField]
        private VRInteractiveItem m_CurrentInteractible;                //The current interactive item
        private VRInteractiveItem m_LastInteractible;                   //The last interactive item

        RaycastHit[] hits = new RaycastHit[10];

        PlatformManager platformManager;
        public Transform rightFingerIndicator;

        // Utility for other classes to get the current interactive item
        public VRInteractiveItem CurrentInteractible
        {
            get { return m_CurrentInteractible; }
        }

        // Utility for other classes to get the current interactive item
        public VRInteractiveItem LastInteractible
        {
            get { return m_LastInteractible; }
        }

        public void ResetInteractable()
        {
            m_CurrentInteractible = null;
        }

        public Reticle GetReticle(bool isRightHand)
        {
            return isRightHand ? m_ReticleR : m_ReticleL;
        }

        private void Start()
        {
            platformManager = ReferenceManagerIndependent.Instance.PlatformManager;
            switch (platformManager.CurrentVRPlatform)
            {
                case VRPlataform.PC:
                    m_LineRendererL.gameObject.SetActive(false);
                    m_ReticleL.gameObject.SetActive(false);
                    break;
                case VRPlataform.Oculus:
                    m_LineRendererL.gameObject.SetActive(false);
                    m_ReticleL.gameObject.SetActive(false);
                    break;
                case VRPlataform.OculusQuest:
                    m_LineRendererL.gameObject.SetActive(false);
                    m_ReticleL.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        private void OnEnable()
        {
            m_VrInput.OnClick += HandleClick;
            m_VrInput.OnUp += HandleUp;
            m_VrInput.OnGrab += HandleGrab;
        }
        
        private void OnDisable ()
        {
            m_VrInput.OnClick -= HandleClick;
            m_VrInput.OnUp -= HandleUp;
            m_VrInput.OnGrab -= HandleGrab;
        }
        
        private void Update()
        {
            // Show the debug ray if required
            if (m_ShowDebugRay)
            {
                Debug.DrawRay(m_Camera.position, m_Camera.forward * m_DebugRayLength, Color.blue, m_DebugRayDuration);
            }
            
            RaycastScheme();            
                             
        }
        
        
        void RaycastScheme()
        {
            switch (platformManager.CurrentVRPlatform)
            {
                case VRPlataform.PC:
                default:
                    if (platformManager.CurrentVRControlScheme == VRControlScheme.Laser)
                        RayCastPositionPerPlatform();
                    else
                        HandGrabberTestInDesktop(isRight: true);
                    break;
                case VRPlataform.Oculus:
                    RayCastPositionPerPlatform(OVRInput.Controller.Active, isRight: true);
                    break;
                case VRPlataform.OculusQuest:
                    if (platformManager.CurrentVRControlScheme == VRControlScheme.Laser)
                        RayCastPositionPerPlatform(OVRInput.Controller.RTouch, isRight: true);
                    else
                        HandGrabber(OVRInput.Controller.RTouch, isRight: true);
                    break;
            }
        }

        void RayCastPositionPerPlatform(OVRInput.Controller controller, bool isRight)
        {           
            Ray ray;

            Vector3 worldStartPoint;
            Vector3 worldEndPoint;     

            Matrix4x4 localToWorld = m_TrackingSpace.localToWorldMatrix;
            Quaternion orientation = OVRInput.GetLocalControllerRotation(controller);

            Vector3 localStartPoint = OVRInput.GetLocalControllerPosition(controller);
            Vector3 localEndPoint = localStartPoint + ((orientation * Vector3.forward) * m_RayLength);

            worldStartPoint = localToWorld.MultiplyPoint(localStartPoint);
            worldEndPoint = localToWorld.MultiplyPoint(localEndPoint);

            // Create new ray
            ray = new Ray(worldStartPoint, worldEndPoint - worldStartPoint);

            EyeRaycast(ray, worldStartPoint, worldEndPoint, isRight: true);
        }

        void RayCastPositionPerPlatform()
        {

            Ray ray;

            Vector3 worldStartPoint;
            Vector3 worldEndPoint;

            worldStartPoint = m_Camera.position;
            worldEndPoint = worldStartPoint + (m_Camera.forward * m_RayLength);
            ray = new Ray(m_Camera.position, m_Camera.forward);

            EyeRaycast(ray, worldStartPoint, worldEndPoint, isRight : true);
        }
        
        private void EyeRaycast(Ray ray, Vector3 worldStartPoint, Vector3 worldEndPoint, bool isRight)
        {            
            
            //first look for UI elements
            hits = Physics.SphereCastAll(ray, m_RayRadius, m_RayLength, m_UILayers);

            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (var item in hits)
                {
                    print(item.transform.name +"_" + item.distance);
                }
                print("_______________________");
            }

#endif

            // Do the raycast forwards to see if we hit an interactive item
            if (hits.Length > 0)
            {
                ProcessRaycastHits(hits, worldStartPoint, out worldEndPoint, isRight);                

                RenderLine(worldStartPoint, worldEndPoint, isRight);

                return;
            }         
            
            //if there is no hit, then look for other colliders
            hits = Physics.SphereCastAll(ray, m_RayRadius, m_RayLength, ~m_ExclusionLayers);

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (var item in hits)
                {
                    print(item.transform.name);
                }
                print("_______________________");
            }
#endif

            // Do the raycast forwards to see if we hit an interactive item
            if (hits.Length > 0)
            {
                ProcessRaycastHits(hits, worldStartPoint, out worldEndPoint, isRight);
            }
            else
            {
                m_CurrentInteractible = null;
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                
                // Position the reticle at default distance.
                Reticle ret = isRight ? m_ReticleR : m_ReticleL;
                if (ret)
                    ret.SetPosition(ray.origin, ray.direction);
            }
             
            RenderLine(worldStartPoint, worldEndPoint, isRight);

        }

        private void HandGrabber(OVRInput.Controller controller, bool isRight)
        {                                   
            Quaternion orientation = OVRInput.GetLocalControllerRotation(controller);
            Matrix4x4 localToWorld = m_TrackingSpace.localToWorldMatrix;

            Vector3 localStartPoint = OVRInput.GetLocalControllerPosition(controller);
            Vector3 localEndPoint = localStartPoint + ((orientation * Vector3.forward) * m_RayRadius);


            //Vector3 worldEndPoint = localToWorld.MultiplyPoint(localEndPoint);
            Vector3 worldEndPoint = rightFingerIndicator.transform.position;

            Collider[] hits = Physics.OverlapSphere(worldEndPoint, m_RayRadius);
            
            // Do the raycast forwards to see if we hit an interactive item
            if (hits.Length > 0)
            {
                ProcessCollisionHits(hits, worldEndPoint, isRight);                
                return;
            }            
            else
            {
                m_CurrentInteractible = null;
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                Reticle ret = isRight ? m_ReticleR : m_ReticleL;
                if (ret)
                    ret.SetPosition(worldEndPoint);
            }
        }

        private void HandGrabberTestInDesktop(bool isRight)
        {
            Vector3 worldStartPoint = Camera.main.transform.position;

            Collider[] hits = Physics.OverlapSphere(worldStartPoint, m_RayRadius);

            // Do the raycast forwards to see if we hit an interactive item
            if (hits.Length > 0)
            {
                ProcessCollisionHits(hits, worldStartPoint, isRight);
                return;
            }
            else
            {
                m_CurrentInteractible = null;
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                Reticle ret = isRight ? m_ReticleR : m_ReticleL;
                if (ret)
                    ret.SetPosition(worldStartPoint);
            }
        }
        
        void ProcessRaycastHits(RaycastHit [] hits, Vector3 worldStartPoint, out Vector3 worldEndPoint, bool isRight)
        {
            VRInteractiveItem interactible = null; //hits[0].transform.GetComponent<VRInteractiveItem>();
            RaycastHit hit = hits[0];
            float closestInteractive = Mathf.Infinity;

            for (int i = 0; i < hits.Length; i++)
            {
                VRInteractiveItem currentItem = hits[i].transform.GetComponent<VRInteractiveItem>();
                if (hits[i].distance < closestInteractive && currentItem)
                {
                    closestInteractive = hits[i].distance;
                    interactible = currentItem; //attempt to get the VRInteractiveItem on the hit object  
                    hit = hits[i];
                }
            }
                      
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
                print(hit.transform.name);
#endif

            worldEndPoint = hit.point == Vector3.zero ? worldStartPoint : hit.point;

            m_CurrentInteractible = interactible;

            // If we hit an interactive item and it's not the same as the last interactive item, then call Over
            if (interactible && interactible != m_LastInteractible)
                HandleOver(interactible);

            // Deactive the last interactive item 
            if (interactible != m_LastInteractible)
                DeactiveLastInteractible();

            m_LastInteractible = interactible;

            // Something was hit, set at the hit position.
            Reticle ret = isRight ? m_ReticleR : m_ReticleL;
            if (ret)
                ret.SetPosition(hit, worldStartPoint);

            OnRaycasthit?.Invoke(hit);
        }

        void ProcessCollisionHits(Collider[] hits, Vector3 worldStartPoint, bool isRight)
        {
            VRInteractiveItem interactible = null;
            Collider hit = hits[0];
            float closestInteractive = Mathf.Infinity;
            
            for (int i = 0; i < hits.Length; i++)
            {
                VRInteractiveItem currentItem;

                if (hits[i].attachedRigidbody)
                    currentItem = hits[i].attachedRigidbody.GetComponent<VRInteractiveItem>();
                else
                    currentItem = hits[i].GetComponent<VRInteractiveItem>();
                float distance = Vector3.Distance(worldStartPoint, currentItem ? currentItem.transform.position : hits[i].transform.position);
                if (distance < closestInteractive && currentItem)
                {
                    closestInteractive = distance;
                    interactible = currentItem; //attempt to get the VRInteractiveItem on the hit object  
                    hit = hits[i];
                }
            }

            m_CurrentInteractible = interactible;

            // If we hit an interactive item and it's not the same as the last interactive item, then call Over
            if (interactible && interactible != m_LastInteractible)
                HandleOver(interactible);

            // Deactive the last interactive item 
            if (interactible != m_LastInteractible)
                DeactiveLastInteractible();

            m_LastInteractible = interactible;

            Reticle ret = isRight ? m_ReticleR : m_ReticleL;
            if (ret)
                ret.SetPosition(worldStartPoint);
        }
        
        void RenderLine(Vector3 worldStartPoint, Vector3 worldEndPoint, bool isRight)
        {
            LineRenderer line = isRight ? m_LineRendererR : m_LineRendererL;
            if (line != null && ShowLineRenderer)
            {
                line.enabled = ShowLineRenderer;
                line.SetPosition(0, worldStartPoint);
                line.SetPosition(1, worldEndPoint);
            }
        }

        public void SetLine(bool show, bool isRight)
        {
            ShowLineRenderer = show;
            LineRenderer line = isRight ? m_LineRendererR : m_LineRendererL;
            line.enabled = show;
        }
        
        private void DeactiveLastInteractible()
        {
            if (m_LastInteractible == null)
                return;

            HandleOut(m_LastInteractible);
            m_LastInteractible = null;
        }

        private void HandleUp()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Up();
        }
                
        private void HandleClick()
        {    
            if (m_CurrentInteractible != null)
            {
                m_CurrentInteractible.Click();
            }
        }

        private void HandleGrab()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Grab();
        }

        void HandleOver(VRInteractiveItem interactible)
        {
            interactible.Over();
            if(m_LastInteractible == null)
                OnOverSomething?.Invoke(interactible);
        }

        void HandleOut(VRInteractiveItem interactible)
        {
            interactible.Out();
            if (m_CurrentInteractible == null)
                OnOutOfEverything?.Invoke(interactible);
        }
    }
}