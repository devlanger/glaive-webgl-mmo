using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCoreEngine
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance
        {
            get;
            private set;
        }

        public virtual bool CanZoom
        {
            get
            {
                return true;
            }
        }

        private enum Type
        {
            ThirdPerson,
            FirstPerson
        }

        [SerializeField]
        private Transform target;

        [SerializeField]
        private float shakeIntensity = 0.5f;

        [SerializeField]
        private Vector3 offset;
        public Vector3 targetRot;

        [SerializeField]
        private LayerMask collisionMask;

        [SerializeField]
        private LayerMask aimMask;

        public float[] zoomLevels;

        private bool isLocked;
        public float collideOffset;
        private float x = 33;
        private float shakeTime;
        private float fov;

        public Vector2 GetMouseAxis()
        {
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        public float GetScrollAxis()
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }

        public void SetZoomLevel(int option)
        {
            maxZoom = zoomLevels[option];
        }

        public float SensitivityX
        {
            get;
            private set;
        }

        public float SensitivityY
        {
            get;
            private set;
        }

        public void ApplyShake()
        {
            shakeTime += 0.15f;
        }

        public Vector3 GetAimingPoint()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 50, aimMask))
            {
                return hit.point;
            }
            else
            {
                return transform.position + transform.forward * 50;
            }
        }

        public Vector3 GetFlatDirection()
        {
            Vector3 forward = transform.forward;
            forward.y = 0;
            return forward.normalized;
        }

        private float y;

        public void SetSensitivityX(float val)
        {
            this.SensitivityX = val;
        }

        public void SetSensitivityY(float val)
        {
            this.SensitivityY = val;
        }

        public void SetFov(float value)
        {
            if (fov == value)
            {
                return;
            }

            fov = value;
        }

        [SerializeField]
        private float turnSpeed;
        public float TurnSpeedY
        {
            get
            {
                return turnSpeed * SensitivityY;
            }

            set
            {
                turnSpeed = value;
            }
        }
        public float TurnSpeedX
        {
            get
            {
                return turnSpeed * SensitivityX;
            }

            set
            {
                turnSpeed = value;
            }
        }

        [SerializeField]
        private float zoomSpeed;
        public float ZoomSpeed
        {
            get
            {
                return zoomSpeed;
            }

            set
            {
                zoomSpeed = value;
            }
        }

        public bool IsLocked
        {
            get
            {
                return isLocked;
            }

            set
            {
                isLocked = value;
            }
        }

        public Transform Target
        {
            get
            {
                return target;
            }

            set
            {
                target = value;
            }
        }

        private float minZoom = 2;
        [SerializeField]
        private float maxZoom = 8;

        [SerializeField]
        private bool lerp;
        private Camera cam;
        private float zoomValue;
        private float collidingValue;

        private bool isColliding;

        public void SetTargetRot(Vector3 euler)
        {
            this.targetRot = euler;
            y = 0;
        }

        private void Awake()
        {
            fov = GetComponent<Camera>().fieldOfView;

            SensitivityX = PlayerPrefs.HasKey("turn_speed_x") ? PlayerPrefs.GetFloat("turn_speed_x") : 1;
            SensitivityY = PlayerPrefs.HasKey("turn_speed_y") ? PlayerPrefs.GetFloat("turn_speed_y") : 1;

            this.cam = GetComponent<Camera>();
            zoomValue = maxZoom;
            rotate = true;

            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void SetRotation(Quaternion rot)
        {
            y = rot.eulerAngles.y;
        }

        public void UpdateState()
        {
            if (Target == null)
                return;

            ThirdPersonUpdate();
        }

        public void UpdateInput()
        {
            if (Target == null)
                return;

            if (rotate && !IsLocked)
            {
                x -= GetMouseAxis().y * TurnSpeedY * 0.02f;
                y += GetMouseAxis().x * TurnSpeedX * 0.02f;
            }

            SetScroll(GetScrollAxis());
        }

        public void SetScroll(float val)
        {
            if (CanZoom)
            {
                zoomValue -= val * ZoomSpeed;
            }
            zoomValue = Mathf.Clamp(zoomValue, minZoom, maxZoom);
        }

        protected virtual void LateUpdate()
        {

        }

        Vector3 camPos;
        [SerializeField]
        private float lerpValue = 0.1f;
        public bool rotate = true;

        private void ThirdPersonUpdate()
        {
            if (cam.fieldOfView != fov)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, 15 * Time.deltaTime);
            }

            camPos = Target.position;

            x = Mathf.Clamp(x, -60, 80);

            offset.z = Mathf.Lerp(offset.z, -zoomValue, 0.2f);

            transform.rotation = GetRotation(false, true);
            Vector3 up = ((Quaternion.Euler(targetRot) * Vector3.up) * (offset.y * (zoomValue / maxZoom)));

            Vector3 pos = transform.rotation * new Vector3(offset.x, 0, offset.z) + camPos + up;

            if (shakeTime > 0)
            {
                shakeTime -= Time.deltaTime;
                pos += new Vector3(UnityEngine.Random.Range(-shakeIntensity, shakeIntensity), UnityEngine.Random.Range(-shakeIntensity, shakeIntensity), UnityEngine.Random.Range(-shakeIntensity, shakeIntensity));
            }

            transform.position = pos;

            Vector3 offsetFromCollision = new Vector3(0, 0.3f, 0);
            CheckCollision(offsetFromCollision);

            for (int i = 0; i < 3; i++)
            {
                Vector3 dir = transform.right;
                switch (i)
                {
                    case 0:
                        dir = transform.right;
                        break;
                    case 1:
                        dir = -transform.right;
                        break;
                    case 2:
                        dir = Vector3.up;
                        break;
                }
                dir *= 0.3f;

                Debug.DrawLine(transform.position, transform.position + dir);

                if (CheckCollision(dir))
                    break;

            }
        }

        public Quaternion GetRotation(bool onlyY, bool useTargetRot)
        {
            if (useTargetRot)
            {
                return Quaternion.Euler(targetRot) * Quaternion.Euler(onlyY ? 0 : x, y, 0);
            }
            else
            {
                return Quaternion.Euler(onlyY ? 0 : x, y, 0);
            }
        }

        public Quaternion GetAttachRotation()
        {
            return Quaternion.Euler(targetRot);
        }

        public bool CheckCollision(Vector3 offset)
        {
            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position - offset, out hit, collisionMask))
            {
                transform.position = new Vector3(hit.point.x + offset.x, hit.point.y + offset.y, hit.point.z + offset.z);

                return true;
            }
            else
            {
                return false;
            }
        }

        public void FaceCameraAt(Vector3 pos)
        {
            Quaternion rot = Quaternion.LookRotation(pos - target.transform.position);
            SetRotation(rot);
        }
    }
}