# 🌬️ AR Digital Twin - Smart Fan

![Unity](https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white)
![Vuforia](https://img.shields.io/badge/Vuforia-000000?style=for-the-badge&logo=vuforia&logoColor=white)
![MQTT](https://img.shields.io/badge/MQTT-660066?style=for-the-badge&logo=mqtt&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

An **Augmented Reality Digital Twin** project to monitor and simulate a Smart Fan in real-time based on the Internet of Things (IoT). This project was developed as a Midterm Exam assignment for the Intelligent Systems course.

---

## 📝 Project Description
This system is a virtual representation (Digital Twin) of a physical fan. By utilizing Augmented Reality (AR) technology, users can point their smartphone or laptop camera at a specific Image Target (marker) to render a 3D model of the fan.

This virtual fan is connected to an IoT network via the MQTT communication protocol. The fan's blade rotation speed in the AR environment adapts automatically and in real-time based on the environmental temperature data sent by a publisher (such as a physical ESP32/LM35 sensor or a Node-RED simulator).

## ✨ Key Features
* **AR Marker Tracking:** Stably tracks and renders 3D objects using the Vuforia Engine.
* **IoT Integration (MQTT):** Connects to a public broker (`broker.emqx.io`) to receive instantaneous data streams with minimal latency.
* **Responsive Automation Logic:** The fan's rotation speed directly adapts to the received temperature conditions.
* **Visual Indicator:** Allows users to easily understand the system's real-time status through interactive visual simulation.

## ⚙️ System Logic (How it Works)
The application acts as a *Subscriber* that continuously monitors a specific MQTT topic. When a temperature data update is received, the system responds based on the following rules:

| Temperature Condition | 3D Fan Status | Animation Speed |
| :--- | :--- | :--- |
| **Temp $\ge$ 30°C** | 🟢 On (Fast) | Maximum Rotation (Fast) |
| **Temp $\ge$ 25°C** | 🟡 On (Normal)| Medium Rotation (Normal) |
| **Temp < 25°C** | 🔴 Off | Blades Stopped |

## 🛠️ Technologies & Specifications
* **Game Engine:** Unity 3D
* **AR SDK:** Vuforia Engine v11.4.4
* **IoT Protocol:** MQTT 
* **MQTT Broker:** `broker.emqx.io` (Public Broker)
* **Programming Language:** C# (AR and MQTT behavior scripting)
* **Testing Tool:** Node-RED / MQTT Explorer (For simulating the temperature data publisher)

## 🚀 How to Run the Project (Installation)
For those who want to run or modify this project:

1. **Clone the Repository:**
   ```bash
   git clone [https://github.com/luthfiakhaira67/AR-Digital-Twin-SmartFan.git](https://github.com/luthfiakhaira67/AR-Digital-Twin-SmartFan.git)
