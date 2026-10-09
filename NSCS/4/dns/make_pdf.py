from reportlab.lib.pagesizes import A4
from reportlab.lib.units import cm
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib import colors
from reportlab.platypus import (
    SimpleDocTemplate, Paragraph, Spacer, Image, PageBreak, HRFlowable
)
from reportlab.platypus.flowables import KeepTogether
from PIL import Image as PILImage
import os

BASE = os.path.dirname(os.path.abspath(__file__))

TASKS = [
    (1, "Welche DNS-Server verwendet mein PC?",
     "IP-Adresse der DNS-Server (resolvectl dns zeigt den tatsächlichen Server hinter 127.0.0.53)."),
    (2, "IP-Adresse von www.htlwrn.ac.at",
     "Forward-Lookup und Überprüfung eines Reverse-DNS-Eintrags."),
    (3, "IP-Adresse von dns.google",
     "Forward-Lookup, Reverse-DNS-Eintrag prüfen und Forward/Reverse vergleichen."),
    (4, "Autoritative Nameserver für orf.at und google.com",
     "NS-Records für beide Domains ermitteln."),
    (5, "IP-Adressen direkt vom autoritativen Nameserver",
     "www.google.com und www.instagram.com — autoritative Antwort erkennbar am Fehlen des 'Non-authoritative answer'-Hinweises."),
    (6, "Mailserver (MX-Records) für orf.at und gmail.com",
     "Name und IP-Adresse der zuständigen Mailserver."),
    (7, "CNAMEs / Aliase für news.orf.at und www.apple.com",
     "CNAME-Kette bis zum finalen A-Record auflösen."),
    (8, "SOA-Metadaten für orf.at und google.com",
     "Seriennummer, Refresh, Retry, Expire, Minimum-TTL aus dem Start-of-Authority-Record."),
]

TEXT_TASKS = [
    (9,
     "Default-TTL-Vergleich: google.com vs. orf.at",
     open(os.path.join(BASE, "9.txt")).read().strip()),
    (10,
     "E-Mail-Adressen der Zone-Admins (google.com und orf.at)",
     open(os.path.join(BASE, "10.txt")).read().strip()),
]

doc = SimpleDocTemplate(
    os.path.join(BASE, "Protokoll_DNS.pdf"),
    pagesize=A4,
    leftMargin=2*cm, rightMargin=2*cm,
    topMargin=2*cm, bottomMargin=2*cm,
)

styles = getSampleStyleSheet()

title_style = ParagraphStyle(
    "title", parent=styles["Title"],
    fontSize=18, spaceAfter=6,
)
subtitle_style = ParagraphStyle(
    "subtitle", parent=styles["Normal"],
    fontSize=11, textColor=colors.HexColor("#444444"), spaceAfter=14,
)
task_heading = ParagraphStyle(
    "task_heading", parent=styles["Heading2"],
    fontSize=13, textColor=colors.HexColor("#1a1a6e"),
    spaceBefore=10, spaceAfter=4,
)
desc_style = ParagraphStyle(
    "desc", parent=styles["Normal"],
    fontSize=10, textColor=colors.HexColor("#333333"),
    spaceAfter=8, leftIndent=6,
)
answer_style = ParagraphStyle(
    "answer", parent=styles["Normal"],
    fontSize=11, backColor=colors.HexColor("#f5f5f5"),
    borderPad=8, leftIndent=10, rightIndent=10,
    spaceBefore=4, spaceAfter=4,
    leading=16,
)

def scaled_image(path, max_width, max_height):
    with PILImage.open(path) as im:
        w, h = im.size
    ratio = min(max_width / w, max_height / h, 1.0)
    return Image(path, width=w * ratio, height=h * ratio)

page_w = A4[0] - 4*cm  # usable width

story = []

# --- Title page header ---
story.append(Paragraph("DNS Command-Line Tools", title_style))
story.append(Paragraph("Protokoll — NSCS 4. Klasse &nbsp;·&nbsp; 26.05.2026 &nbsp;·&nbsp; Prof. Fuxreiter", subtitle_style))
story.append(HRFlowable(width="100%", thickness=1.5, color=colors.HexColor("#1a1a6e"), spaceAfter=16))

# --- Screenshot tasks ---
for num, heading, description in TASKS:
    img_path = os.path.join(BASE, f"{num}.png")

    block = [
        Paragraph(f"Aufgabe {num}: {heading}", task_heading),
        Paragraph(description, desc_style),
    ]

    if os.path.exists(img_path):
        img = scaled_image(img_path, max_width=page_w, max_height=18*cm)
        block.append(img)

    block.append(Spacer(1, 0.5*cm))
    story.append(KeepTogether(block))
    story.append(HRFlowable(width="100%", thickness=0.5, color=colors.lightgrey, spaceAfter=8))

# --- Text-answer tasks ---
for num, heading, answer_text in TEXT_TASKS:
    block = [
        Paragraph(f"Aufgabe {num}: {heading}", task_heading),
        Paragraph(answer_text.replace("\n", "<br/>"), answer_style),
        Spacer(1, 0.5*cm),
    ]
    story.append(KeepTogether(block))
    story.append(HRFlowable(width="100%", thickness=0.5, color=colors.lightgrey, spaceAfter=8))

doc.build(story)
print("PDF erstellt:", os.path.join(BASE, "Protokoll_DNS.pdf"))
