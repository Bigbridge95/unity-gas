using System.Collections.Generic;
using AttributeSystem.Authoring;
using AttributeSystem.Components;
using UnityEngine;

namespace AttributeSystem.Events.Authoring
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Attribute Event Handler/Clamp Attribute")]
    public class ClampAttributeEventHandler : AbstractAttributeEventHandler
    {

        [SerializeField] private AttributeScriptableObject m_PrimaryAttribute;
        [SerializeField] private AttributeScriptableObject m_MaxAttribute;
        public override void PreAttributeChange(AttributeSystemComponent attributeSystem, List<AttributeValue> prevAttributeValues, ref List<AttributeValue> currentAttributeValues)
        {
            var attributeCacheDict = attributeSystem.mAttributeIndexCache;
            ClampAttributeToMax(m_PrimaryAttribute, m_MaxAttribute, currentAttributeValues, attributeCacheDict);
        }

        private void ClampAttributeToMax(AttributeScriptableObject Attribute1, AttributeScriptableObject Attribute2, List<AttributeValue> attributeValues, Dictionary<AttributeScriptableObject, int> attributeCacheDict)
        {
            if (attributeCacheDict.TryGetValue(Attribute1, out var primaryAttributeIndex)
                && attributeCacheDict.TryGetValue(Attribute2, out var maxAttributeIndex))
            {
                var primaryAttribute = attributeValues[primaryAttributeIndex];
                var maxAttribute = attributeValues[maxAttributeIndex];

                // Clamp current and base values
                if (primaryAttribute.CurrentValue > maxAttribute.CurrentValue) primaryAttribute.CurrentValue = maxAttribute.CurrentValue;
                if (primaryAttribute.BaseValue > maxAttribute.BaseValue) primaryAttribute.BaseValue = maxAttribute.BaseValue;
                attributeValues[primaryAttributeIndex] = primaryAttribute;
            }
        }
    }
}