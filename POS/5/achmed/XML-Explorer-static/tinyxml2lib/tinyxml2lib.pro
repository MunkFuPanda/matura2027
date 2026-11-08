# Statische Bibliothek von TinyXml2 (Kopie des Projekts, Erweiterung)
QT -= gui

TEMPLATE = lib
TARGET = tinyxml2

CONFIG += c++17
CONFIG += staticlib

# DLL-Symbole exportieren (Windows) bzw. Sichtbarkeit setzen

DESTDIR = $$OUT_PWD

HEADERS += \
    tinyxml2.h

SOURCES += \
    tinyxml2.cpp
