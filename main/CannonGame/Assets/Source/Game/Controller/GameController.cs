/* © 2023 - Greg Waller.  All rights reserved. */

using System.Collections;
using UnityEngine;
using UnityEditor;

namespace LRG.Master
{
    using LRG.Data;
    using LRG.UI;
    using LRG.Game;

    public class GameController : MonoBehaviour
    {
        public static string DevelopedBy = "Greg Waller";
        public static int CopyrightYear = 2023;

        public static VersionData CurrentVersion { get; private set; } = new VersionData(0, 0, 1, 0);

        [Header("UI/UX")]
        [SerializeField] private InputController _inputController = null;
        [SerializeField] private Cannon _cannon = null;

        public Cannon Cannon => _cannon;

        public virtual void Start()
        {
            Application.targetFrameRate = 120;

            // pools and factories
            //EffectGenerator.Instance.RegisterMaster(this);
            ProjectileFactory.Instance.Initialize();

            Debug.Log($"Initializing GameController v{CurrentVersion}...");
            Debug.Assert(_inputController != null, "InputController must be specified");
            //Debug.Assert(_desktop != null, "Console must be specified");

            Run(_load_scene());
        }

        public Coroutine Run(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        public virtual void Quit(bool toMenu)
        {
            //AudioLibrary.StopMusic();

            #region Set Tracker Components
            //_loadScreen.Show(true);
            //_tracker[TrackerComponent.CampsiteLauncher].SetActive(false);
            #endregion

#if _AUTOSAVE_ENABLED
            Save();
#endif

            // TODO: Purge game state

            if (toMenu)
            {
                // TODO: Transition to secondary scene
                //DataManager.Instance.Transition_Async("MainMenu");
            }
            else
            {
                _exit_application();
            }
        }

        #region Startup

        private IEnumerator _load_scene()
        {
            yield return _setup_scene();
            yield return _init_scene();
            yield return _present_scene();
        }

        #endregion

        #region Scene Setup

        private IEnumerator _setup_scene()
        {
            yield return _setup_user_interface();
            //_loadScreen.Show(true);

            yield return _setup_world();
            yield return _setup_postprocessing();
            yield return _setup_audio();
        }

        private IEnumerator _setup_user_interface()
        {
            Debug.Log("Setting up User Interface...");

            // ----- GUI
            //yield return _desktop.Initialize(this);
            //_loadScreen = _desktop.GetPanel<LoadScreen_ScreenPanel>();
            //yield return null;

            // ----- USER INPUT
            _inputController.RegisterGameController(this);
            //FeedbackMessage.Instance.Initialize(this, _desktop);
            yield return null;
        }

        private IEnumerator _setup_world()
        {
            yield return null;
        }

        private IEnumerator _setup_postprocessing()
        {
            yield return null;
        }

        private IEnumerator _setup_audio()
        {
            Debug.Log("Setting up AudioLibrary...");

            //AudioLibrary.Instance.Initialize(this, InputController);

            yield return null;
        }

        #endregion

        #region Game Init

        private IEnumerator _init_scene()
        {
            yield return _init_postprocessing();
            yield return _init_audio();
            yield return _init_character();
        }

        private IEnumerator _init_postprocessing()
        {
            Debug.Log("Initializing Post-Processing...");

            //Sky.updateMode.Override(EnvironmentUpdateMode.OnDemand);
            //Sky.updateMode.overrideState = true;

            // TODO: apply post processing settings
            // bool dof = DataManager.Instance.Settings.DepthOfField;
            // if (DOF != null)
            //     DOF.active = dof;
            // 
            // bool motionBlur = DataManager.Instance.Settings.MotionBlur;
            // if (MotionBlur != null)
            //     MotionBlur.active = motionBlur;
            // 
            // bool volumetricFog = DataManager.Instance.Settings.VolumetricFog;
            // if (Fog != null)
            //     Fog.enableVolumetricFog.overrideState = volumetricFog;

            yield return null;
        }

        private IEnumerator _init_audio()
        {
            Debug.Log("Initializing Audio...");
            //AudioLibrary.Play(SongID._02_otherworldly_dream);
            yield return null;
        }

        private IEnumerator _init_character()
        {
            Debug.Log("Initializing player interface");

            //_pc.RegisterMaster(this, _inputController);

            //_inputController.AttachFollowCamera(_pc);

            //unit.Stance = EquipState.Sheathed;
            //unit.Body.Warp(new Vector3(0.0f, -100.0f, 0.0f), HexDirection.North.ToQuaternion(), false);
            //unit.Body.transform.parent = GameObject.Find("Environment/Campsite").transform;
            //unit.Body.gameObject.SetLayer(LayerMask.NameToLayer("Campsite_Stage"), true);
            //InputController.AddHUDComponents(unit, HUDComponentFlags.Halo);
            //unit.Body.EnableCloth(true);
            //unit.Attributes[Attribute.ActionPoints].Current.Fill();

            yield return null;
        }

        #endregion

        #region Finalization

        private IEnumerator _present_scene()
        {
            yield return _save_scene();
            yield return _finalize_scene();
        }

        private IEnumerator _save_scene()
        {
#if _AUTOSAVE_ENABLED || _DEVMODE_NEW
            FeedbackMessage.Log("Saving scene...");
            Save();
#endif

            yield return null;
        }

        private IEnumerator _finalize_scene()
        {
            Debug.Log("Load complete.  Transferring control to player.");

            //foreach (NearSpaceEntity entity in Navigation.TrackedEntities)
            //{
            //    if (entity.name.Contains("drone") || entity.name.Contains("sleip"))
            //        continue;
            //
            //    Core.ExecuteCommand("chart {drone.01} (bnavd find " + entity.name + ")");
            //}

            //InputController.MainCamera.Lock(false);

            //_loadScreen.Show(false);
            //EventService.Instance.Broadcast(new MasterLoaded_MasterEvent());

            yield return null;
        }

        #endregion

        protected void _exit_application()
        {
            Debug.Log("Exiting application...");

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}