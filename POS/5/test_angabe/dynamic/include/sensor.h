#ifndef SENSOR_H
#define SENSOR_H

#include "sensoren_global.h"
#include <QString>

class SENSOREN_EXPORT Sensor
{
public:
    virtual ~Sensor();
    virtual QString einheit() const = 0;
};

class SENSOREN_EXPORT TemperaturSensor : public Sensor
{
public:
    QString einheit() const override;
};

class SENSOREN_EXPORT DistanzSensor : public Sensor
{
public:
    QString einheit() const override;
};

#endif // SENSOR_H

