#include "sensor.h"

Sensor::~Sensor() = default;

QString TemperaturSensor::einheit() const
{
    return QStringLiteral("°C");
}

QString DistanzSensor::einheit() const
{
    return QStringLiteral("m");
}