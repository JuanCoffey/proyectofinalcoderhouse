using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;

namespace SciFiArsenal
{
    public class SciFiFireProjectile : MonoBehaviour
    {
        [SerializeField]
        public GameObject[] projectiles;
        [Header("Missile spawns at attached game object")]
        public Transform spawnPosition0;
        public Transform spawnPosition1;
        public Transform spawnPosition2;
        public byte CurrentWeaponSelected;
        public byte AmmoWeapon0;
        public byte AmmoWeapon1;
        public byte AmmoWeapon2;

        public GameObject WeaponIdleParticle0;
        public GameObject WeaponIdleParticle1;
        public GameObject WeaponIdleParticle2;


        [HideInInspector]
        public int currentProjectile = 0;
        public float speed = 1000;
        public float Weapon0CoolDown = 0;
        public float Weapon1CoolDown = 0;
        public float Weapon2CoolDown = 0;
        public Image imgWeapon0Cooldown;
        public Image imgWeapon1Cooldown;
        public Image imgWeapon2Cooldown;


        //    MyGUI _GUI;
        //  SciFiButtonScript selectedProjectileButton;

        void Start()
        {
            CurrentWeaponSelected = 0;
            AmmoWeapon0 = 3;
            AmmoWeapon1 = 2;
            AmmoWeapon2 = 2;
        }

        RaycastHit hit;

        void Update()
        {
            if (Weapon0CoolDown > 0)
            {
                Weapon0CoolDown -= Time.deltaTime;
                imgWeapon0Cooldown.fillAmount = 1 - (Weapon0CoolDown / 5);
            }
            if (Weapon1CoolDown > 0)
            {
                Weapon1CoolDown -= Time.deltaTime;
                imgWeapon1Cooldown.fillAmount = 1 - (Weapon1CoolDown / 3.5f);
            }
            if (Weapon2CoolDown > 0)
            {
                Weapon2CoolDown -= Time.deltaTime;
                imgWeapon2Cooldown.fillAmount = 1 - (Weapon2CoolDown / 4.2f);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                nextEffect();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                nextEffect();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                previousEffect();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                previousEffect();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000f))
                    {
                        if (!checkIfOnCooldown())
                        {
                            FireCurrentWeapon();
                        }
                    }
                }
            }

            Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100, Color.yellow);

        }

        private void FireCurrentWeapon()
        {
            Transform spawnPosition = null;
            switch (CurrentWeaponSelected)
            {
                case 0:
                    if (AmmoWeapon0 == 0)
                    {
                        return;
                    }
                    Weapon0CoolDown = 5;
                    spawnPosition = spawnPosition0;
                    imgWeapon0Cooldown.fillAmount = 0;
                    break;
                case 1:
                    if (AmmoWeapon1 == 0)
                    {
                        return;
                    }
                    Weapon1CoolDown = 3.5f;
                    spawnPosition = spawnPosition1;
                    imgWeapon1Cooldown.fillAmount = 0;
                    break;
                case 2:
                    if (AmmoWeapon2 == 0)
                    {
                        return;
                    }
                    Weapon2CoolDown = 4.2f;
                    spawnPosition = spawnPosition2;
                    imgWeapon2Cooldown.fillAmount = 0;
                    break;
                default:
                    spawnPosition = spawnPosition0;
                    Weapon0CoolDown = 4;
                    break;
            }
            GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject; //Spawns the selected projectile
            projectile.transform.LookAt(hit.point); //Sets the projectiles rotation to look at the point clicked
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed); //Set the speed of the projectile by applying force to the rigidbody


            discountAmmo();
        }

        private void discountAmmo()
        {
            switch (CurrentWeaponSelected)
            {
                case 0:
                    AmmoWeapon0--;
                    imgWeapon0Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().text = AmmoWeapon0 + "♦";
                    if (AmmoWeapon0 == 0)
                    {
                        imgWeapon0Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
                    }
                    break;
                case 1:
                    AmmoWeapon1--;
                    imgWeapon1Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().text = AmmoWeapon1 + "♦";
                    if (AmmoWeapon1 == 0)
                    {
                        imgWeapon1Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
                    }
                    break;
                case 2:
                    AmmoWeapon2--;
                    imgWeapon2Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().text = AmmoWeapon2 + "♦";
                    if (AmmoWeapon2 == 0)
                    {
                        imgWeapon2Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
                    }
                    break;
                default:
                    break;
            }
        }

        public void PickUpAmmo(byte ammoCode, byte ammoCount)
        {


            switch (ammoCode)
            {
                case 0:
                    AmmoWeapon0 += ammoCount;
                    imgWeapon0Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().text = AmmoWeapon0 + "♦";
                    imgWeapon0Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
                    break;
                case 1:
                    AmmoWeapon1 += ammoCount;
                    imgWeapon1Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().text = AmmoWeapon1 + "♦";
                    imgWeapon1Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
                    break;
                case 2:
                    AmmoWeapon2 += ammoCount;
                    imgWeapon2Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().text = AmmoWeapon2 + "♦";
                    imgWeapon2Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
                    break;
                default:
                    break;
            }
        }

        private bool checkIfOnCooldown()
        {
            bool isOnCooldown = false;

            switch (CurrentWeaponSelected)
            {
                case 0:
                    isOnCooldown = Weapon0CoolDown > 0;
                    break;
                case 1:
                    isOnCooldown = Weapon1CoolDown > 0;
                    break;
                case 2:
                    isOnCooldown = Weapon2CoolDown > 0;
                    break;
                default:
                    break;
            }
            return isOnCooldown;
        }



        public void nextEffect() //Changes the selected projectile to the next. Used by UI
        {
            if (currentProjectile < projectiles.Length - 1)
                currentProjectile++;
            else
                currentProjectile = 0;
            //selectedProjectileButton.getProjectileNames();
        }

        public void previousEffect() //Changes selected projectile to the previous. Used by UI
        {
            if (currentProjectile > 0)
                currentProjectile--;
            else
                currentProjectile = projectiles.Length - 1;
            //selectedProjectileButton.getProjectileNames();
        }

        public void AdjustSpeed(float newSpeed) //Used by UI to set projectile speed
        {
            speed = newSpeed;
        }
    }
}