using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.Authoring;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour, DefaultInputActions.IPlayerAbilitiesActions
{
    public AbstractAbilityScriptableObject[] Abilities;

    public AbstractAbilityScriptableObject[] InitialisationAbilities;
    private AbilitySystemCharacter abilitySystemCharacter;

    private AbstractAbilitySpec[] abilitySpecs;

    private DefaultInputActions playerInput;
    public Image[] Cooldowns;

    // Start is called before the first frame update
    void Start()
    {
        this.abilitySystemCharacter = GetComponent<AbilitySystemCharacter>();
        var spec = Abilities[0].CreateSpec(this.abilitySystemCharacter);
        this.abilitySystemCharacter.GrantAbility(spec);
        ActivateInitialisationAbilities();
        GrantCastableAbilities();
        playerInput = new DefaultInputActions();
        playerInput.PlayerAbilities.SetCallbacks(this);
        playerInput.PlayerAbilities.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < Cooldowns.Length; i++)
        {
            var durationRemaining = this.abilitySpecs[i].CheckCooldown();
            if (durationRemaining.TotalDuration > 0)
            {
                var percentRemaining = durationRemaining.TimeRemaining / durationRemaining.TotalDuration;
                Cooldowns[i].fillAmount = 1 - percentRemaining;
            }
            else
            {
                Cooldowns[i].fillAmount = 1;
            }
        }
    }

    void ActivateInitialisationAbilities()
    {
        for (var i = 0; i < InitialisationAbilities.Length; i++)
        {
            var spec = InitialisationAbilities[i].CreateSpec(this.abilitySystemCharacter);
            this.abilitySystemCharacter.GrantAbility(spec);
            StartCoroutine(spec.TryActivateAbility());
        }
    }

    void GrantCastableAbilities()
    {
        this.abilitySpecs = new AbstractAbilitySpec[Abilities.Length];
        for (var i = 0; i < Abilities.Length; i++)
        {
            var spec = Abilities[i].CreateSpec(this.abilitySystemCharacter);
            this.abilitySystemCharacter.GrantAbility(spec);
            this.abilitySpecs[i] = spec;
        }
    }

    public void UseAbility(int i)
    {
        var spec = abilitySpecs[i];
        StartCoroutine(spec.TryActivateAbility());
    }

    public void OnFire1(InputAction.CallbackContext context)
    {
        UseAbility(0);
    }

    public void OnFire2(InputAction.CallbackContext context)
    {
        UseAbility(1);
    }
}
