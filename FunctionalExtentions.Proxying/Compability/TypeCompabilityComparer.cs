using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Proxying.Compability
{
    internal class TypeCompabilityComparer : ICompabilityComparer
    {
        private List<ICompabilityComparer> _comparers;

        private const BindingFlags All =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Static |
            BindingFlags.Instance;

        public TypeCompabilityComparer()
        {
            _comparers = new List<ICompabilityComparer>();
        }

        public MemberTypes TargetMemberType => MemberTypes.All;

        public bool IsCompatible(object one, object other)
        {
            ThrowHelper.ThrowIfNull(one, nameof(one));
            ThrowHelper.ThrowIfNull(other, nameof(other));

            Type firstType, secondType;

            firstType = one is Type ? (Type)one : one.GetType();
            secondType = other is Type ? (Type)other : other.GetType();

            var firstTypeMembers = firstType.GetMembers(All);
            var secondTypeMembers = secondType.GetMembers(All);

            Dictionary<MemberInfo, MemberInfo> memberPairs = new Dictionary<MemberInfo, MemberInfo>();

            foreach (var member in firstTypeMembers)
            {
                var otherMember = secondTypeMembers
                    .FirstOrDefault(x => x.Name == member.Name && x.MemberType == member.MemberType);
                if (otherMember != null)
                    memberPairs.Add(member, otherMember);
            }

            foreach (var memberPair in memberPairs)
            {
            }

            return false;
        }
    }
}