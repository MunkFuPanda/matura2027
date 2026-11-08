# Dynamische Bibliothek von TinyXml2 (Aufgabe)
QT -= gui

TEMPLATE = lib
TARGET = tinyxml2

CONFIG += c++17
CONFIG += libPIC

# DLL-Symbole exportieren (nur Windows; tinyxml2.h waehlt selbst die Sichtbarkeit)
win32:DEFINES += TINYXML2_LIB

# DLL/Import-Lib ins Build-Verzeichnis der App legen,
# damit die App die Bibliothek zur Laufzeit findet.
DESTDIR = $$OUT_PWD/../explorer

HEADERS += \
    tinyxml2.h

SOURCES += \
    tinyxml2.cpp
