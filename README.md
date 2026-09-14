# Arena Duel — Unity 6 Rebuild

Mobile-first Unity 6 URP prototype focused on one production hero: Ember.

## Open and build

1. Install Unity Hub and Unity **6000.0.30f1** (or a compatible Unity 6 LTS editor) with Android Build Support, SDK/NDK tools and OpenJDK.
2. Open this folder as a Unity project.
3. Wait for packages and scripts to import.
4. Unity automatically runs `ArenaDuelProjectBuilder` once. If needed, run **Arena Duel > Build/Rebuild Playable Project**.
5. Open `Assets/Scenes/Boot.unity` and press Play.
6. For Android, use **File > Build Profiles**, select Android, then build an APK/AAB.

The Unity-ready static asset is at `Assets/Art/Characters/Ember/Ember.fbx`. The editable Blender 5.2 source, validated GLB and asset audit are preserved under `SourceAssets/Ember` so Unity does not try to import the `.blend` file through a mismatched local Blender installation.

## Included milestone

- Redesigned landscape Main Menu with Ember showcase.
- Ember-only Hero Select.
- Open World Coming Soon modal.
- Settings and local JSON save data.
- Third-person arena, virtual joystick, jump, basic attack and three skills.
- Modular movement, health, combat, cooldowns, status effects and AI.
- Ember versus Ember, timer, victory/defeat and battle HUD.
- Low/Medium/High/Ultra quality selection.
- Static Ember FBX; animation is intentionally deferred.

## Important limitation

Unity is not installed in the authoring environment, so the project has not yet been imported, compiled, played, or built for Android. The setup script is designed to create all generated scenes and prefabs inside Unity; run it after the first import and address any editor-version package migration prompts.
