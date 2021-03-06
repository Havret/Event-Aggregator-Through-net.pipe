﻿using System.Threading;
using Rhino.Mocks;
using TeoVincent.EA.Client.UnitTests.EventMocks;
using TeoVincent.EA.Client.UnitTests.ListenerMocks;
using TeoVincent.EA.Common;
using TeoVincent.EA.Common.Events.Tts;
using Xunit;

namespace TeoVincent.EA.Client.UnitTests
{
    public class InternalEventAggeregatorTester
    {
        private readonly IEventAggregator internalEventAggregator;

        public InternalEventAggeregatorTester()
        {
            var syncContexts = new SynchronizationContext();
            internalEventAggregator = new InternalEventAggregator(syncContexts);
        }
        
        [Fact]
        public void Subscribe_Publish_Call_Handle_Validate()
        {
            // 1) arrange
            var listener = MockRepository.GenerateStub<IListener<Simple_MockEvent>>();
            internalEventAggregator.Subscribe(listener);
            var e = new Simple_MockEvent();

            // 2) act
            internalEventAggregator.Publish(e);

            // 3) assert
            listener.AssertWasCalled(l => l.Handle(e));
        }

        [Fact]
        public void Subscribe_Publish_Was_Not_Call_Validate()
        {
            // 1) arrange
            var listener = MockRepository.GenerateStub<IListener<Simple_MockEvent>>();
            internalEventAggregator.Subscribe(listener);
            var e = new Simple_MockEvent();
            var anotherEvent = new Simple_MockEvent();

            // 2) act
            internalEventAggregator.Publish(e);

            // 3) assert
            listener.AssertWasNotCalled(l => l.Handle(anotherEvent));
        }

        [Fact]
        public void Subscribe_Publish_Difirent_Events_Call_Handle_Validate()
        {
            // 1) arrange
            var listener = MockRepository.GenerateStub<IListener<Simple_MockEvent>>();
            internalEventAggregator.Subscribe(listener);
            var e1 = new Simple_MockEvent();
            var e2 = new Simple_MockEvent();

            // 2) act
            internalEventAggregator.Publish(e1);
            internalEventAggregator.Publish(e2);

            // 3) assert
            listener.AssertWasCalled(l => l.Handle(e1));
            listener.AssertWasCalled(l => l.Handle(e2));
        }

        [Fact]
        public void Subscribe_Publish_Generic_Call_Handle_Validate()
        {
            // 1) arrange
            var listener = MockRepository.GenerateStub<IListener<Simple_MockEvent>>();
            var e = new Simple_MockEvent();

            // 2) act
            internalEventAggregator.Subscribe(listener);
            internalEventAggregator.Publish(e);

            // 3) assert
            listener.AssertWasCalled(x => x.Handle(e));
        }

        [Fact]
        public void Subscribe_Publish_Generic_Not_Null_Event_Vaslidate()
        {
            // 1) arrange
            var listener = new Simple_MockListener();
            internalEventAggregator.Subscribe(listener);
            
            // 2) act
            internalEventAggregator.Publish<Simple_MockEvent>();

            // 3) assert
            Assert.NotNull(listener.Event);
        }

        [Fact]
        public void Subscribe_Publish_The_Same_Info_In_Event_Validate()
        {
            // 1) arrange
            var listener = new Simple_MockListener();
            internalEventAggregator.Subscribe(listener);

            // 2) act
            internalEventAggregator.Publish<Simple_MockEvent>();

            string expected = "Simple_MockEvent";
            string actual = listener.Event.ChildType;

            // 3) assert
            Assert.Equal(expected, actual);
        }
    }
}
