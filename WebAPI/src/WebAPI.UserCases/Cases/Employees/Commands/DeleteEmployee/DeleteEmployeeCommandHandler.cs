﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebAPI.Entities.Models;
using WebAPI.Infrastructure.Interfaces.DataAccess;
using WebAPI.UserCases.Common.Exceptions;

namespace WebAPI.UserCases.Cases.Employees.Commands.DeleteEmployee
{
    /// <summary>
    /// Implements a handler for the employee delete command.
    /// </summary>
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, string>
    {
        private readonly ICrudRepository<Employee> _repository;

        public DeleteEmployeeCommandHandler(ICrudRepository<Employee> repository) =>
            _repository = repository;

        /// <summary>
        /// Handles a request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Returns string about success.</returns>
        public async Task<string> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = new Employee() { Id = request.EmployeeId };

            if (employee == null || employee.Id != request.EmployeeId)
            {
                throw new NotFoundException(nameof(employee), request.EmployeeId);
            }
            
            var success = true;
            try
            {
                _repository.Delete(employee.Id);
            }
            catch (Exception)
            {
                success = false;
            }

            return await Task.FromResult(success ? "Deleted successful" : "Deletion failed");
        }
    }
}