using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		namespace Node
		{
			namespace Detail
			{
				public abstract class NotRootNode : Detail.BaseNode
				{
					public ReadOnlyCollection<BaseService> services { get; private set; } = null;
					public ReadOnlyCollection<BaseDecorator> decorators { get; private set; } = null;
					protected List<BaseService> m_services { get; private set; } = null;
					protected List<BaseDecorator> m_decorators { get; private set; } = null;

					public bool isAllTrueDecorators
					{
						get
						{
							bool isResult = true;
							foreach (var e in decorators)
								isResult &= e.IsPredicate();
							return isResult;
						}
					}

					public NotRootNode() : base()
					{
						m_services = new List<BaseService>();
						m_decorators = new List<BaseDecorator>();
						services = new ReadOnlyCollection<BaseService>(m_services);
						decorators = new ReadOnlyCollection<BaseDecorator>(m_decorators);
					}

					protected void LoadDecoratorAndService(CashContainer.NotRootCashContainer container)
					{
						foreach(var serviceInfo in container.serviceClasses)
						{
							m_services.Add((BaseService)System.Activator.CreateInstance(
								System.Type.GetType(serviceInfo.className)));
							m_services.Back().LoadInterval(serviceInfo.callInterval);
						}

						foreach (var decorator in container.decoratorClasses)
						{
							m_decorators.Add((BaseDecorator)System.Activator.CreateInstance(
								System.Type.GetType(decorator)));
						}
					}

					protected void CloneDecoratorAndService(NotRootNode node)
					{
						foreach (var service in node.services)
						{
							m_services.Add(service.ReturnNewThisClass());
							m_services.Back().LoadInterval(service.callInterval);
						}

						foreach (var decorator in node.decorators)
							m_decorators.Add(decorator.ReturnNewThisClass());
					}
				}
			}
		}
	}
}