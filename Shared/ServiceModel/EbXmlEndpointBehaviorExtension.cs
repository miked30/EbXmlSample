using System;
using System.ServiceModel.Configuration;

namespace Shared.ServiceModel
{
    public class EbXmlEndpointBehaviorExtension : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new EbXmlEndpointBehaviour();
        }

        public override Type BehaviorType
        {
            get { return typeof(EbXmlEndpointBehaviour); }
        }
    }
}