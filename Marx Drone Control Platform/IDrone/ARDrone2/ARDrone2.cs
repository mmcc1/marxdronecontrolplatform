using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using AR.Drone.Client;
using AR.Drone.Client.Commands;
using AR.Drone.Client.Configuration;
using AR.Drone.Client.Configuration.Sections;
using AR.Drone.Data;
using AR.Drone.Data.Navigation.Native;
using AR.Drone.Media;
using AR.Drone.Video;

namespace Marx_Drone_Control_Platform.IDrone.ARDrone2
{
    class ARDrone2
    {
        private readonly DroneClient _droneClient;
        private readonly PacketRecorder _packetRecorderWorker;
        private readonly VideoPacketDecoderWorker _videoPacketDecoderWorker;
        private uint _frameNumber;
        private VideoFrame _frame;
        private Bitmap _frameBitmap;
        private NavigationPacket _navigationPacket;

        public ARDrone2()
        {
            _videoPacketDecoderWorker = new VideoPacketDecoderWorker(PixelFormat.BGR24, true, OnVideoPacketDecoded);
            _videoPacketDecoderWorker.Start();

            string path = string.Format("flight_{0:yyyy-MM-dd-HH-mm}.ardrone", DateTime.Now);
            var stream = new FileStream(path, FileMode.OpenOrCreate);
            _packetRecorderWorker = new PacketRecorder(stream);
            _packetRecorderWorker.Start();

            _droneClient = new DroneClient();
            _droneClient.NavigationPacketAcquired += OnNavigationPacketAcquired;
            _droneClient.VideoPacketAcquired += OnVideoPacketAcquired;
            _droneClient.ConfigurationUpdated += OnConfigurationUpdated;
            _droneClient.Active = true;

            //tmrStateUpdate.Enabled = true;
            //tmrVideoUpdate.Enabled = true;
        }

        private void OnNavigationPacketAcquired(NavigationPacket packet)
        {
            if (_packetRecorderWorker.IsAlive)
                _packetRecorderWorker.EnqueuePacket(packet);

            _navigationPacket = packet;
        }

        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            if (_packetRecorderWorker.IsAlive)
                _packetRecorderWorker.EnqueuePacket(packet);
            if (_videoPacketDecoderWorker.IsAlive)
                _videoPacketDecoderWorker.EnqueuePacket(packet);
        }

        private void OnConfigurationUpdated(DroneConfiguration configuration)
        {
            if (configuration.Video.Codec != VideoCodecType.H264_360P ||
                configuration.Video.BitrateCtrlMode != VideoBitrateControlMode.Dynamic)
            {
                _droneClient.Send(configuration.Video.Codec.Set(VideoCodecType.H264_360P).ToCommand());
                _droneClient.Send(configuration.Video.BitrateCtrlMode.Set(VideoBitrateControlMode.Dynamic).ToCommand());
            }
        }

        private void OnVideoPacketDecoded(VideoFrame frame)
        {
            _frame = frame;
        }

        public void EnginesArm()
        {
            _droneClient.Active = true;
        }

        public void EnginesDisarm()
        {
            _droneClient.Active = false;
        }

        public void RequestConfiguration()
        {
            _droneClient.RequestConfiguration();
        }

        public void Emergency()
        {
            _droneClient.Emergency();
        }

        public void ResetEmergency()
        {
            _droneClient.ResetEmergency();
        }

        public void FlatTrim()
        {
            _droneClient.FlatTrim();
        }

        public void Takeoff()
        {
            _droneClient.Takeoff();
        }

        public void Hover()
        {
            _droneClient.Hover();
        }

        public void Land()
        {
            _droneClient.Land();
        }

        public void SwitchCam()
        {
            DroneConfiguration configuration = _droneClient.Configuration;
            ATCommand command = configuration.Video.Channel.Set(VideoChannelType.Next).ToCommand();
            _droneClient.Send(command);
        }

        public void Roll(float roll)
        {
            _droneClient.Progress(ProgressiveMode.CombinedYaw, roll);
        }

        public void Pitch(float pitch)
        {
            _droneClient.Progress(ProgressiveMode.CombinedYaw, pitch);
        }

        public void Yaw(float yaw)
        {
            _droneClient.Progress(ProgressiveMode.CombinedYaw, yaw);
        }

        public void Gaz(float gaz)
        {
            _droneClient.Progress(ProgressiveMode.CombinedYaw, gaz);
        }


    }
}
