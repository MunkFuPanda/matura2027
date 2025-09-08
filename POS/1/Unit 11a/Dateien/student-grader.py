"""
author: Markus Spitzer
file: student-grader.py
"""


def read_students():
    students = []

    try:
        while True:
            student_name = input("Schüler: ")

            if len(student_name) > 30:
                print("Invalid, must be <= 30 characters!")
                continue

            students.append(student_name)

    except EOFError:
        return students


def read_course(students):
    course_name = input("\nGegenstand: ")
    student_scores = {}
    score_sum = 0

    print("")

    for student_name in students:
        score = -1

        while score < 0:
            try:
                score = float(input("Schüler {}: ".format(student_name)))
            except:
                print("Invalid, retry:")

        score_sum += score
        student_scores[student_name] = score

    return (course_name, student_scores, round(score_sum / len(student_scores)), 1)


noten = {
    90: 1,
    78: 2,
    62: 3,
    50: 4,
    0: 5,
}

students = []

print("Die Eingabe kann jeweils mit CTRL-D abgebrochen werden!\n")
print("Bitte die Schüler eingeben:\n")

student_names = read_students()

print("<CTRL-D>\n\nBitte die Gegenstände und die jeweiligen Punkte eingeben:")

try:
    while True:
        students.append(read_course(student_names))
except EOFError:
    print("<CTRL-D>\n\nAuswertungen\n============")

    for info in students:
        print("\nGegenstand {}\n{}\n".format(info[0], "-" * (11 + len(info[0]))))

        for rang, (key, value) in enumerate(
            sorted(info[1].items(), key=lambda x: x[1], reverse=True)
        ):
            grade = -1

            for i, v in noten.items():
                if value >= i:
                    grade = v
                    break

            print(
                "Schüler: {0:<30} Rang: {1:>3} Punkte: {2:>5} Note: {3}".format(
                    key, rang + 1, value, grade
                )
            )

        print(
            "\n{0} Schüler {1:>25}".format(len(info[1]), "Schnitt: {}".format(info[2]))
        )

    print(
        "{}Reihung der Gegenstände basierend auf den Schnitt der Punkte\n{}\n".format(
            "\n" * 2, "=" * 60
        )
    )

    for i, info in enumerate(sorted(students, key=lambda x: x[2], reverse=True)):
        print("{0}. {1:<6} {2}".format(i + 1, info[0] + ":", info[2]))
