using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Proxying.Compability
{
    public class GeneralCompabilityComparer : CompabilityComparerBase
    {
        private List<ICompabilityComparer> _comparers;

        public GeneralCompabilityComparer()
        {
            _comparers = new List<ICompabilityComparer>();
        }

        public override MemberTypes TargetMemberType => MemberTypes.All;

        public override bool IsCompatible(object one, object other)
        {
            var firstTypeMembers = GetAllMembers(one);
            var secondTypeMembers = GetAllMembers(other);
            var memberPairs = new Dictionary<MemberInfo, MemberInfo>();

            foreach (MemberInfo member in firstTypeMembers)
            {
                MemberInfo otherMember = secondTypeMembers
                    .FirstOrDefault(x => x.Name == member.Name && x.MemberType == member.MemberType);
                if (otherMember != null)
                    memberPairs.Add(member, otherMember);
            }

            foreach (KeyValuePair<MemberInfo, MemberInfo> memberPair in memberPairs)
            {
                ICompabilityComparer comparer = _comparers.FirstOrDefault(x => x.TargetMemberType == memberPair.Key.MemberType);
                if (comparer != null && !comparer.IsCompatible(memberPair.Key, memberPair.Value))
                {
                    return false;
                }
            }

            return true;
        }

        private IEnumerable<MemberInfo> GetAllMembers(object source)
        {
            ThrowHelper.ThrowIfNull(source, nameof(source));
            var type = source is Type ? (Type)source : source.GetType();
            return type.GetMembers(All);
        }
    }
}