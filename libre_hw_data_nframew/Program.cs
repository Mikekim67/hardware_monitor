using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading;
using LibreHardwareMonitor.Hardware;

class Program
{
    static void Main(string[] args)
    {
        // 아두이노 연결을 위한 시리얼 포트 설정
        Console.WriteLine("Port Number ex) COM0 : ");
        string port_num = Console.ReadLine(); // 문자열 읽기
        Console.WriteLine("Selected Port : " + port_num);

        SerialPort arduinoPort = new SerialPort(port_num, 115200);
        arduinoPort.Open();

        // LibreHardwareMonitor 설정
        Computer computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true
        };

        computer.Open();
        while (true) // 무한 루프 시작
        {
            DateTime currentTime = DateTime.Now; // 현재 시간 가져오기
            string Time = $"{currentTime.Hour}:{currentTime.Minute}:{currentTime.Second}";
            Console.WriteLine(Time); // 콘솔 출력
            arduinoPort.WriteLine(Time);  // Arduino로 전송

            foreach (var hardware in computer.Hardware)
            {
            hardware.Update();
            string hardwareName = "-";
            hardwareName = hardware.Name.Replace("Intel Core ", "").Trim(); // 문자열에서 "Intel Core" 제거
            hardwareName = hardwareName.Replace("NVIDIA GeForce ", "").Trim(); // 문자열에서 "NVIDIA Geforce" 제거
            hardwareName = hardwareName.Replace(" SUPER", "s").Trim(); // 문자열에서 " SUPER" "s"로 변경
            hardwareName = hardwareName.Replace(" Intel(R) Iris(R) Xe Graphics", "Iris Xe").Trim(); // 문자열에서 " SUPER" "s"로 변경
            

            Console.WriteLine(hardwareName); // 콘솔창에 출력
            arduinoPort.WriteLine(hardwareName);// 아두이노 포트로 보내기

                foreach (var sensor in hardware.Sensors)
                {
                    
                    if (sensor.SensorType == SensorType.Temperature)
                    {
                    //Console.WriteLine($"{sensor.Name} : {sensor.Value.GetValueOrDefault()} 'C");// 모든 데이터 출력 (테스트용)
                    if (sensor.Name == "Core Average") 
                        {
                            string cpu_core_t = "-"; 
                            cpu_core_t = $"{(int)Math.Round(sensor.Value.GetValueOrDefault())} 'C";
                            Console.WriteLine($"{sensor.Name} : {cpu_core_t}");  // 콘솔 출력
                            arduinoPort.WriteLine(cpu_core_t);  // Arduino로 전송
                        }
                        if (sensor.Name == "GPU Core") 
                        {
                            string gpu_core_t = "-";
                            //gpu_core_t = $"{(int)Math.Round(sensor.Value.GetValueOrDefault())} 'C";
                            Console.WriteLine($"{sensor.Name} : {gpu_core_t}");  // 콘솔 출력
                            arduinoPort.WriteLine(gpu_core_t);  // Arduino로 전송
                        }
                  
                    }

                    if (sensor.SensorType == SensorType.Load)
                    {
                   // Console.WriteLine($"{sensor.Name} : {sensor.Value.GetValueOrDefault()} %");// 모든 데이터 출력 (테스트용)
                    if (sensor.Name == "CPU Total")
                        {
                            string cpu_core_l = "-";
                            cpu_core_l = $"{(int)Math.Round(sensor.Value.GetValueOrDefault())} %";
                            Console.WriteLine($"{sensor.Name} : {cpu_core_l}");  // 콘솔 출력
                            arduinoPort.WriteLine(cpu_core_l);  // Arduino로 전송

                        }
                        if (sensor.Name == "GPU Core")
                        {
                            string gpu_core_l = "-";
                            gpu_core_l = $"{(int)Math.Round(sensor.Value.GetValueOrDefault())} %";
                            Console.WriteLine($"{sensor.Name} : {gpu_core_l}");  // 콘솔 출력
                            arduinoPort.WriteLine(gpu_core_l);  // Arduino로 전송
                        }
                    }
                }
            }

            Thread.Sleep(500); // 0.1초 대기 (원하는 주기로 조절 가능)
        }

        // 프로그램이 종료될 때 실행 (실제로는 도달하지 않음)
        // arduinoPort.Close();
        // computer.Close();
    }
}
