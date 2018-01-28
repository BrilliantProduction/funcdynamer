using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Proxying.Compability.Comparers
{
    public class GeneralCompatibilityComparer : CompatibilityComparerBase
    {
        private List<ICompatibilityComparer> _comparers;

        public GeneralCompatibilityComparer()
        {
            _comparers = new List<ICompatibilityComparer>();
        }

        public IList<ICompatibilityComparer> CompabilityComparers => _comparers;

        public bool AddComparer(ICompatibilityComparer comparer)
        {
            var result = false;

            if (!_comparers.Any(x => x.TargetMemberType == comparer.TargetMemberType))
            {
                _comparers.Add(comparer);
                result = true;
            }

            return result;
        }

        public void ClearComparers()
        {
            _comparers.Clear();
        }

        public bool Remove(ICompatibilityComparer comparer)
        {
            bool result = false;
            int index;

            if ((index = _comparers.FindIndex(x => ReferenceEquals(x, comparer))) >= 0)
                _comparers.RemoveAt(index);

            return result;
        }

        public bool Remove(MemberTypes memberType)
        {
            bool result = false;
            int index;

            if ((index = _comparers.FindIndex(x => x.TargetMemberType == memberType)) >= 0)
                _comparers.RemoveAt(index);

            return result;
        }

        public ICompatibilityComparer GetCompabilityComparerByMemberType(MemberTypes memberType)
        {
            return _comparers.FirstOrDefault(x => x.TargetMemberType == memberType);
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
                ICompatibilityComparer comparer = _comparers.FirstOrDefault(x => x.TargetMemberType == memberPair.Key.MemberType);
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