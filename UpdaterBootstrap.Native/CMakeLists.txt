cmake_minimum_required(VERSION 3.20)
project(UpdaterBootstrap)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# vcpkg integration
find_package(nlohmann_json CONFIG REQUIRED)

add_executable(UpdaterBootstrap 
    src/main.cpp
    src/ConfigReader.cpp
    src/Logger.cpp
    src/ProcessManager.cpp
)

target_link_libraries(UpdaterBootstrap PRIVATE nlohmann_json::nlohmann_json)

# Windows subsystem
if(WIN32)
    set_target_properties(UpdaterBootstrap PROPERTIES WIN32_EXECUTABLE TRUE)
endif()
