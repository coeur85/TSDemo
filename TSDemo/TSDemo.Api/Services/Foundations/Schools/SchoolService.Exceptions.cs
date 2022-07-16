using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TSDemo.Api.Models.Schools;
using TSDemo.Api.Models.Schools.Exceptions;
using Xeptions;

namespace TSDemo.Api.Services.Foundations.Schools
{
    public partial class SchoolService
    {
        private delegate ValueTask<School> ReturningSchoolFunction();
        private delegate IQueryable<School> ReturningSchoolsFunction();

        private async ValueTask<School> TryCatch(ReturningSchoolFunction returningSchoolFunction)
        {
            try
            {
                return await returningSchoolFunction();
            }
            catch (NullSchoolException nullSchoolException)
            {
                throw CreateAndLogValidationException(nullSchoolException);
            }
            catch (InvalidSchoolException invalidSchoolException)
            {
                throw CreateAndLogValidationException(invalidSchoolException);
            }
            catch (SqlException sqlException)
            {
                var failedSchoolStorageException =
                    new FailedSchoolStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedSchoolStorageException);
            }
            catch (NotFoundSchoolException notFoundSchoolException)
            {
                throw CreateAndLogValidationException(notFoundSchoolException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsSchoolException =
                    new AlreadyExistsSchoolException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsSchoolException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidSchoolReferenceException =
                    new InvalidSchoolReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidSchoolReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedSchoolStorageException =
                    new FailedSchoolStorageException(databaseUpdateException);

                throw CreateAndLogDependecyException(failedSchoolStorageException);
            }
            catch (Exception exception)
            {
                var failedSchoolServiceException =
                    new FailedSchoolServiceException(exception);

                throw CreateAndLogServiceException(failedSchoolServiceException);
            }
        }

        private IQueryable<School> TryCatch(ReturningSchoolsFunction returningSchoolsFunction)
        {
            try
            {
                return returningSchoolsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedSchoolStorageException =
                    new FailedSchoolStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedSchoolStorageException);
            }
            catch (Exception exception)
            {
                var failedSchoolServiceException =
                    new FailedSchoolServiceException(exception);

                throw CreateAndLogServiceException(failedSchoolServiceException);
            }
        }

        private SchoolValidationException CreateAndLogValidationException(Xeption exception)
        {
            var schoolValidationException =
                new SchoolValidationException(exception);

            this.loggingBroker.LogError(schoolValidationException);

            return schoolValidationException;
        }

        private SchoolDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var schoolDependencyException = new SchoolDependencyException(exception);
            this.loggingBroker.LogCritical(schoolDependencyException);

            return schoolDependencyException;
        }

        private SchoolDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var schoolDependencyValidationException =
                new SchoolDependencyValidationException(exception);

            this.loggingBroker.LogError(schoolDependencyValidationException);

            return schoolDependencyValidationException;
        }

        private SchoolDependencyException CreateAndLogDependecyException(
            Xeption exception)
        {
            var schoolDependencyException = new SchoolDependencyException(exception);
            this.loggingBroker.LogError(schoolDependencyException);

            return schoolDependencyException;
        }

        private SchoolServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var schoolServiceException = new SchoolServiceException(exception);
            this.loggingBroker.LogError(schoolServiceException);

            return schoolServiceException;
        }
    }
}